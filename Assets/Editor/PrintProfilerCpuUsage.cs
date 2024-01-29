using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditorInternal.Profiling;
using UnityEditor.Profiling;

public class PrintProfilerCpuUsage
{

    struct Info
    {
        public string name;
        public float totalPercent;
        public float selfPercent;
        public int calls;
        public int gcMemory;
        public float totalTime;
        public float selfTime;
    }

    static Info GetInfo(HierarchyFrameDataView frameData, int id)
    {
        Info info = new Info();
        info.name = frameData.GetItemName(id);
        info.totalPercent = frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnTotalPercent);
        info.calls = (int)frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnCalls);
        info.gcMemory = (int)frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnGcMemory);
        info.totalTime = frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnTotalTime);
        info.selfTime = frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnSelfTime);
        return info;
    }

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
            List<int> targetCacheList = new List<int>();

            frameData.GetItemDescendantsThatHaveChildren(frameData.GetRootItemID(), parentCacheList);
            foreach (int parentId in parentCacheList)
            {
                frameData.GetItemChildren(parentId, childrenCacheList);

                foreach (int id in childrenCacheList)
                {

                    if (frameData.GetItemPath(id) == selectedPath)
                    {
                        frameData.GetItemChildren(id, targetCacheList);

                        foreach (var targetId in targetCacheList)
                        {
                            var info = GetInfo(frameData, targetId);
                            Debug.Log($"{info.name} {info.totalPercent}% {info.selfPercent}% {info.calls} {info.gcMemory}B {info.totalTime} {info.selfTime}");
                        }

                        return;
                    }
                }
            }
        }
    }
}
