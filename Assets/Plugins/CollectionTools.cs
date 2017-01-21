using System.Collections.Generic;

public class CollectionTools
{
	public static void Init<T>(ref List<T> list, IEnumerable<T> initContent = null)
	{
		if (list != null)
		{
			list.Clear();
		}
		else
		{
			UnityEngine.Profiling.Profiler.BeginSample("CollectionTools.Init > new List");
			list = new List<T>();
			UnityEngine.Profiling.Profiler.EndSample();
		}

		if (initContent != null)
		{
			UnityEngine.Profiling.Profiler.BeginSample("CollectionTools.Init > AddRange");
			list.AddRange(initContent);
			UnityEngine.Profiling.Profiler.EndSample();
		}
	}

	// Return true if already init
	public static bool AssertInit<T1>(ref List<T1> list, List<T1> initContent = null)
	{
		if (list != null)
		{
			return true;
		}

		CollectionTools.Init(ref list, initContent);
		return false;
	}

	public static void InitOrClear<T1, T2>(ref Dictionary<T1, T2> dict, Dictionary<T1, T2> initContent = null)
	{
		if (dict != null)
		{
			dict.Clear();
		}
		else
		{
			UnityEngine.Profiling.Profiler.BeginSample("CollectionTools.Init > new Dictionary");
			dict = new Dictionary<T1, T2>();
			UnityEngine.Profiling.Profiler.EndSample();
		}

		if (initContent != null)
		{
			UnityEngine.Profiling.Profiler.BeginSample("CollectionTools.Init > dict.Add");
			dict = new Dictionary<T1, T2>();
			Dictionary<T1, T2>.Enumerator initContentEnumerator = initContent.GetEnumerator();
			while (initContentEnumerator.MoveNext())
			{
				T1 key = initContentEnumerator.Current.Key;
				T2 val = initContentEnumerator.Current.Value;
				dict.Add(key, val);
			}
			UnityEngine.Profiling.Profiler.EndSample();
		}
	}

	// Return true if already init
	public static bool AssertInit<T1, T2>(ref Dictionary<T1, T2> dict, Dictionary<T1, T2> initContent = null)
	{
		if (dict != null)
		{
			return true;
		}

		CollectionTools.InitOrClear(ref dict, initContent);
		return false;
	}
}
