using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using System.Text;

public class IntToStringTest : MonoBehaviour
{
	int count0 = 0;
	string countText0 = "";

	int count1 = 0;
	string countText1 = "";

	StringBuilder sb = new StringBuilder();

	void Update()
	{
		// 28byte
		Profiler.BeginSample("ToString");
		countText0 = count0.ToString();
		count0++;
		Profiler.EndSample();

		// 84byte
		Profiler.BeginSample("StringBuilder");
		sb.Length = 0;
		sb.Append(count1);
		countText1 = sb.ToString();
		count1++;
		Profiler.EndSample();
	}

	void OnGUI()
	{
		GUILayout.Label(countText0);
		GUILayout.Label(countText1);
	}
}
