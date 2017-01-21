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
	public int InitPlayerHeartCount;
    public int MaxPlayerHeartCount;
	public Color[] PlayerColors;
	public Sprite[] HazardSprites;

	public GameObject StepPrefab;
	public GameObject HazardPrefab;

	public AnimationCurve DifficultyCurve;

	public Sprite GetHazardSprite(Hazard.HazardType hazardType)
	{
		string hazardTypeName = hazardType.ToString();

		for (int i = 0; i < this.HazardSprites.Length; i++)
		{
			Sprite hazardSprite = this.HazardSprites[i];

			if (hazardSprite.name == hazardTypeName)
			{
				return hazardSprite;
			}
		}

		Debug.LogErrorFormat("Unable to find {0} hazard type sprite!", hazardTypeName);
		return null;
	}
}
