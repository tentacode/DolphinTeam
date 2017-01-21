using UnityEditor;
using UnityEngine;

public class EditorTools
{
	[MenuItem("Tools/Save Project %s")]
	public static void SaveProject()
	{
		AssetDatabase.SaveAssets();
	}

	[MenuItem("Tools/GCCollect %g")]
	public static void GCCollect()
	{
		System.GC.Collect();
	}

	[MenuItem("Tools/Set all ScriptableObjects dirty")]
	public static void SetAllScriptableObjectsDirty()
	{
		Object[] scriptableObjects = Resources.LoadAll(string.Empty, typeof(ScriptableObject));
		for (int i = 0; i < scriptableObjects.Length; ++i)
		{
			ScriptableObject scriptableObject = scriptableObjects[i] as ScriptableObject;
			EditorUtility.SetDirty(scriptableObject);
		}
	}

	public static Vector2 GetMainGameViewSize()
	{
		System.Type type = System.Type.GetType("UnityEditor.GameView,UnityEditor");
		System.Reflection.MethodInfo getSizeOfMainGameViewMethod = type.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
		object res = getSizeOfMainGameViewMethod.Invoke(null, null);
		return (Vector2)res;
	}
}
