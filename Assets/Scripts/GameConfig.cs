using UnityEngine;

[CreateAssetMenu]
public class GameConfig : ScriptableObject
{
	public float LevelSpeed;
	public float MoveUnit;
	public int ColumnCount;
	public int MinStepCount;
	public int MaxStepCount;
	public float StepOffset;
	public int MinHazardCount;
	public int MaxHazardCount;

	public GameObject StepPrefab;
	public GameObject HazardPrefab;
}
