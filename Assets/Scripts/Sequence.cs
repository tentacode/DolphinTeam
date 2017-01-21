using UnityEngine;

[CreateAssetMenu]
public class Sequence : ScriptableObject
{
	public int WaveGroupCount;
	public AnimationCurve MinDifficultyCurve;
	public AnimationCurve MaxDifficultyCurve;
	public float VerticalSize;
}
