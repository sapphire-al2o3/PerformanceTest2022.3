using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class TryCatchTest : MonoBehaviour
{
    void Start()
    {
		{
			Profiler.BeginSample("exception");

			try
			{
				int i = 10;
				int d = 0;
				int r = i / d;
			}
			catch
			{

			}

			Profiler.EndSample();
		}

		{
			Profiler.BeginSample("no exception");

			try
			{
				int i = 10;
				int d = 1;
				int r = i / d;
			}
			catch
			{

			}

			Profiler.EndSample();
		}
    }
}
