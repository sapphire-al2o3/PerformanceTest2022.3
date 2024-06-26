using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.Profiling;
using System.Text;

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
        info.selfPercent = frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnSelfPercent);
        info.calls = (int)frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnCalls);
        info.gcMemory = (int)frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnGcMemory);
        info.totalTime = frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnTotalTime);
        info.selfTime = frameData.GetItemColumnDataAsFloat(id, HierarchyFrameDataView.columnSelfTime);
        return info;
    }

    [MenuItem("Editor/Print Profiler CPU Usage (Children)")]
    static void PrintChildren()
    {
        var profiler = EditorWindow.GetWindow<ProfilerWindow>();
        if (profiler == null)
        {
            return;
        }

        string selectedPath = ProfilerDriver.selectedPropertyPath;
        if (string.IsNullOrEmpty(selectedPath))
        {
            return;
        }

        int frame = (int)profiler.selectedFrameIndex;

        using (var frameData = ProfilerDriver.GetHierarchyFrameDataView(frame, 0, HierarchyFrameDataView.ViewModes.MergeSamplesWithTheSameName, HierarchyFrameDataView.columnTotalPercent, false))
        {
            List<int> childrenCacheList = new List<int>();
            List<int> parentCacheList = new List<int>();

            frameData.GetItemDescendantsThatHaveChildren(frameData.GetRootItemID(), parentCacheList);
            foreach (int parentId in parentCacheList)
            {
                frameData.GetItemChildren(parentId, childrenCacheList);

                foreach (int id in childrenCacheList)
                {

                    if (frameData.GetItemPath(id) == selectedPath)
                    {
                        List<int> targetCacheList = new List<int>();
                        frameData.GetItemChildren(id, targetCacheList);

                        StringBuilder sb = new StringBuilder();

                        foreach (var targetId in targetCacheList)
                        {
                            var info = GetInfo(frameData, targetId);
                            sb.AppendLine($"{info.name} {info.totalPercent}% {info.selfPercent}% {info.calls} {info.gcMemory} {info.totalTime} {info.selfTime}");
                        }

                        string text = sb.ToString();
                        Debug.Log(text);
                        EditorGUIUtility.systemCopyBuffer = text;
                        return;
                    }
                }
            }
        }
    }

    [MenuItem("Editor/Print Profiler CPU Usage")]
    static void Print()
    {
        var profiler = EditorWindow.GetWindow<ProfilerWindow>();
        if (profiler == null)
        {
            return;
        }

        string selectedPath = ProfilerDriver.selectedPropertyPath;
        if (string.IsNullOrEmpty(selectedPath))
        {
            return;
        }

        int frame = (int)profiler.selectedFrameIndex;

        using (var frameData = ProfilerDriver.GetHierarchyFrameDataView(frame, 0, HierarchyFrameDataView.ViewModes.MergeSamplesWithTheSameName, HierarchyFrameDataView.columnTotalPercent, false))
        {
            List<int> childrenCacheList = new List<int>();
            List<int> parentCacheList = new List<int>();

            frameData.GetItemDescendantsThatHaveChildren(frameData.GetRootItemID(), parentCacheList);
            foreach (int parentId in parentCacheList)
            {
                frameData.GetItemChildren(parentId, childrenCacheList);

                foreach (int id in childrenCacheList)
                {

                    if (frameData.GetItemPath(id) == selectedPath)
                    {
                        var info = GetInfo(frameData, id);
                        string text = $"{info.name} {info.totalPercent}% {info.selfPercent}% {info.calls} {info.gcMemory} {info.totalTime} {info.selfTime}";

                        Debug.Log(text);
                        EditorGUIUtility.systemCopyBuffer = text;
                        return;
                    }
                }
            }
        }
    }

    [MenuItem("Editor/Print Profiler CPU Usage (Filter)")]
    static void PrintByFilter()
    {
        var profiler = EditorWindow.GetWindow<ProfilerWindow>();
        if (profiler == null)
        {
            return;
        }

        var controller = profiler.GetFrameTimeViewSampleSelectionController(ProfilerWindow.cpuModuleIdentifier);
        string searchName = controller.sampleNameSearchFilter;
        Debug.Log(searchName);

        int frame = (int)profiler.selectedFrameIndex;

        using (var frameData = ProfilerDriver.GetHierarchyFrameDataView(frame, 0, HierarchyFrameDataView.ViewModes.MergeSamplesWithTheSameName, HierarchyFrameDataView.columnTotalPercent, false))
        {
            List<int> childrenCacheList = new List<int>();
            List<int> parentCacheList = new List<int>();

            StringBuilder sb = new StringBuilder();
            frameData.GetItemDescendantsThatHaveChildren(frameData.GetRootItemID(), parentCacheList);
            foreach (int parentId in parentCacheList)
            {
                frameData.GetItemChildren(parentId, childrenCacheList);

                foreach (int id in childrenCacheList)
                {

                    if (frameData.GetItemName(id).Contains(searchName))
                    {
                        var info = GetInfo(frameData, id);
                        sb.AppendLine($"{info.name} {info.totalPercent}% {info.selfPercent}% {info.calls} {info.gcMemory} {info.totalTime} {info.selfTime}");
                    }
                }
            }

            string text = sb.ToString();
            Debug.Log(text);
            EditorGUIUtility.systemCopyBuffer = text;
        }
    }
}
