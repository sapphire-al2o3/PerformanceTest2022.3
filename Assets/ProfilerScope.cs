using System;
using UnityEngine.Profiling;

public struct ProfilerScope : IDisposable
{
	public ProfilerScope(string name)
	{
		Profiler.BeginSample(name);
	}

	public void Dispose()
	{
		Profiler.EndSample();
	}
}
