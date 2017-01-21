using System.Collections.Generic;
using UnityEngine;

public static class CollectionExtensions
{
	public static void Shuffle<T>(this List<T> list)
	{
		System.Random rng = new System.Random();
		int n = list.Count;
		while (n > 1)
		{
			--n;
			int k = rng.Next(n + 1);
			T val = list[k];
			list[k] = list[n];
			list[n] = val;
		}
	}

	public static T PullAt<T>(this List<T> list, int index)
	{
		T element = list[index];
		list.RemoveAt(index);
		return element;
	}

	public static T PullRandom<T>(this List<T> list)
	{
		if (list.Count > 0)
		{
			return list.PullAt(Random.Range(0, list.Count));
		}
		else
		{
			return default(T);
		}
	}

	public static T GetRandom<T>(this List<T> list)
	{
		if (list.Count > 0)
		{
			return list[Random.Range(0, list.Count)];
		}
		else
		{
			return default(T);
		}
	}

	public static T GetRandom<T>(this T[] array)
	{
		if (array.Length > 0)
		{
			return array[Random.Range(0, array.Length)];
		}
		else
		{
			return default(T);
		}
	}

	public static T GetFirst<T>(this List<T> list)
	{
		if (list.Count > 0)
		{
			return list[0];
		}
		else
		{
			return default(T);
		}
	}

	public static T GetFirst<T>(this T[] array)
	{
		if (array.Length > 0)
		{
			return array[0];
		}
		else
		{
			return default(T);
		}
	}

	public static T GetLast<T>(this List<T> list)
	{
		if (list.Count > 0)
		{
			return list[list.Count - 1];
		}
		else
		{
			return default(T);
		}
	}

	public static T GetLast<T>(this T[] array)
	{
		if (array.Length > 0)
		{
			return array[array.Length - 1];
		}
		else
		{
			return default(T);
		}
	}

	public delegate float GetWeightValue<T>(T obj);

	public static T GetWeightedRandom<T>(this List<T> list, GetWeightValue<T> getWeightValueDlg)
	{
		// Compute total weight
		float totalProbWeight = 0;
		float weight;
		for (int i = 0; i < list.Count; ++i)
		{
			weight = getWeightValueDlg(list[i]);
			if (weight <= 0)
			{
				// Unpickable element
				continue;
			}

			totalProbWeight += weight;
		}

		// Pick random total weight
		float rdmWeight = Random.Range(0f, totalProbWeight);

		// Search for matching element
		totalProbWeight = 0;
		for (int i = 0; i < list.Count; ++i)
		{
			weight = getWeightValueDlg(list[i]);
			if (weight <= 0)
			{
				// Unpickable element
				continue;
			}

			totalProbWeight += weight;

			if (rdmWeight <= totalProbWeight)
			{
				return list[i];
			}
		}

		// No element found (shouldn't happen!)
		Debug.LogError("Unable to find a match after probability weigth computation! (totalProbWeight=" + totalProbWeight + ")");
		return default(T);
	}

	public static T GetWeightedRandom<T>(this Dictionary<T, float> dict)
	{
		// Compute total weight
		float totalProbWeight = 0;
		float weight;
		foreach (KeyValuePair<T, float> kvp in dict)
		{
			weight = kvp.Value;
			if (weight <= 0)
			{
				// Unpickable element
				continue;
			}

			totalProbWeight += weight;
		}

		// Pick random total weight
		float rdmWeight = Random.Range(0f, totalProbWeight);

		// Search for matching element
		totalProbWeight = 0;
		foreach (KeyValuePair<T, float> kvp in dict)
		{
			if (kvp.Value <= 0)
			{
				// Unpickable element
				continue;
			}

			totalProbWeight += kvp.Value;

			if (rdmWeight <= totalProbWeight)
			{
				return kvp.Key;
			}
		}

		// No element found (shouldn't happen!)
		Debug.LogError("Unable to find a match after probability weigth computation! (totalProbWeight=" + totalProbWeight + ")");
		return default(T);
	}

	public static bool AssertKeyIsDefined<T1, T2>(this Dictionary<T1, T2> dict, T1 key, T2 defaultValue = default(T2), bool replaceValue = false)
	{
		if (dict.ContainsKey(key))
		{
			// Key already defined

			if (replaceValue)
			{
				// Replace
				dict[key] = defaultValue;
			}

			return true;
		}

		// Add
		dict.Add(key, defaultValue);
		return false;
	}

	public static void AddOrReplace<T1, T2>(this Dictionary<T1, T2> dict, T1 key, T2 value)
	{
		if (dict.ContainsKey(key))
		{
			dict[key] = value;
		}
		else
		{
			dict.Add(key, value);
		}
	}

	public static int IndexOf<T>(this T[] array, T element)
	{
		return System.Array.IndexOf(array, element);
	}

	public static bool Contains<T>(this T[] array, T element)
	{
		return array.IndexOf(element) >= 0;
	}

	public static bool Contains<T>(this List<T> list, T element, IEqualityComparer<T> equalityComparer)
	{
		for (int i = 0; i < list.Count; ++i)
		{
			T listElement = list[i];
			if (equalityComparer.Equals(listElement, element))
			{
				return true;
			}
		}

		return false;
	}

	public static void Sort<T>(this T[] array, System.Comparison<T> comparer)
	{
		System.Array.Sort(array, comparer);
	}

	// Returns true is element was added
	public static bool AddIfNotAlreadyContained<T>(this List<T> list, T element)
	{
		if (!list.Contains(element))
		{
			list.Add(element);
			return true;
		}

		return false;
	}

	public static bool TryGet<T>(this List<T> list, int elementIndex, out T element)
	{
		if (elementIndex >= 0 && elementIndex < list.Count)
		{
			element = list[elementIndex];
			return true;
		}

		element = default(T);
		return false;
	}

	public static void RemoveRange<T>(this List<T> list, List<T> removeList)
	{
		for (int i = 0; i < removeList.Count; ++i)
		{
			list.Remove(removeList[i]);
		}
	}

	public static void RemoveRange<T>(this List<T> list, T[] removeArray)
	{
		for (int i = 0; i < removeArray.Length; ++i)
		{
			list.Remove(removeArray[i]);
		}
	}
}
