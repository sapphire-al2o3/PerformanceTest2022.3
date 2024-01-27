using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditorInternal.Profiling;
using UnityEditor.Profiling;

public class PrintProfilerCpuUsage
{
    [MenuItem("Editor/Print Profiler CPU Usage")]
    static void Print()
    {
        var profiler = EditorWindow.GetWindow<ProfilerWindow>();

        string selectedPath = ProfilerDriver.selectedPropertyPath;
        int frame = (int)profiler.selectedFrameIndex;

        var controller = profiler.GetFrameTimeViewSampleSelectionController(ProfilerWindow.cpuModuleIdentifier);
        var selection = controller.selection;

        if (selection != null)
        {
            //Debug.Log(selection.frameIndex);
            //Debug.Log(selection.sampleDisplayName);
        }

        //Debug.Log($"{frame} {selectedPath}");

        using (var frameData = ProfilerDriver.GetHierarchyFrameDataView(frame, 0, HierarchyFrameDataView.ViewModes.MergeSamplesWithTheSameName, HierarchyFrameDataView.columnName, false))
        {
            List<int> childrenCacheList = new List<int>();
            List<int> parentCacheList = new List<int>();
            //Debug.Log(frameData.sampleCount);
            frameData.GetItemDescendantsThatHaveChildren(frameData.GetRootItemID(), parentCacheList);
            foreach (int parentId in parentCacheList)
            {
                frameData.GetItemChildren(parentId, childrenCacheList);
                foreach (int id in childrenCacheList)
                {
                    if (frameData.GetItemPath(id) == selectedPath)
                    {
                        float totalPercent = frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnTotalPercent);
                        float selfPercent = frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnSelfPercent);
                        float calls = frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnCalls);
                        float gcMemory = frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnGcMemory);
                        float totalTime = frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnTotalTime);
                        float selfTime = frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnSelfTime);
                        Debug.Log($"{frameData.GetItemName(id)} {totalPercent}% {selfPercent}% {calls} {gcMemory}B {totalTime} {selfTime}");
                        break;
                    }
                }
                //Debug.Log(frameData.GetItemName(parentId));
            }
        }
    }
}
