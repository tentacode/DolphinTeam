using UnityEngine;

[CreateAssetMenu]
public class Sequence : ScriptableObject
{
	public int WaveGroupCount;
	public AnimationCurve MinDifficultyCurve;
	public AnimationCurve MaxDifficultyCurve;
	public bool NoHazardHiding;
	public float WaveSize;
}
