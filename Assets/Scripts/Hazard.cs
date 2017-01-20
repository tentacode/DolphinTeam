using UnityEngine;

public class Hazard : MonoBehaviour
{
	[SerializeField]
	private Color[] colors;
	[SerializeField]
	private Sprite[] sprites;
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	private HazardType type;

	[System.Serializable]
	public enum HazardType
	{
		Mine,
		Flying,
		Shark,
	}

	[System.Serializable]
	public class Config
	{
		public HazardType Type;
		public int ColorIndex;
	}

	public void Init(Hazard.Config hazardConfig)
	{
		this.spriteRenderer.sprite = this.sprites[(int)hazardConfig.Type];
		this.spriteRenderer.color = this.colors[hazardConfig.ColorIndex];

		this.type = hazardConfig.Type;
	}
}
