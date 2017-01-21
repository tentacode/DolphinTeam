using UnityEngine;

[CreateAssetMenu]
public class GameConfig : ScriptableObject
{
	public float LevelSpeed;
	public float MoveUnit;
	public int ColumnCount;
	public float WaveOffset;
	public int InitPlayerHeartCount;
    public int MaxPlayerHeartCount;
	public float JumpDuration;
	public Color[] PlayerColors;
	public Color NeutralColor;
	public Sprite[] HazardSprites;
	public WaveGroupPattern[] WaveGroupPatterns;
	public Sequence[] Sequences;

	public GameObject HazardPrefab;
	public GameObject WavePrefab;

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
