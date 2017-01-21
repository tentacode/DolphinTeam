using UnityEngine;

[CreateAssetMenu]
public class GameConfig : ScriptableObject
{
	public float LevelSpeed;
	public float MoveUnit;
	public int ColumnCount;
	public int MinPatternGroupCount;
	public int MaxPatternGroupCount;
	public float WaveOffset;
	public int InitPlayerHeartCount;
	public Color[] PlayerColors;
	public Sprite[] HazardSprites;
	public PatternGroup[] PatternGroups;

	public GameObject HazardPrefab;

	public AnimationCurve MinDifficultyCurve;
	public AnimationCurve MaxDifficultyCurve;

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
