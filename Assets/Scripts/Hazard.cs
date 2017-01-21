using UnityEngine;

public class Hazard : AdvancedMonoBehaviour
{
	[SerializeField]
	private GameConfig gameConfig;
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	public HazardType Type { get; private set; }

	[System.Serializable]
	public enum HazardType
	{
		None,
		Mine,
		Helicopter,
		Shark,
		Heart,
		Treasure,
	}

	[System.Serializable]
	public class Config
	{
		public HazardType Type;
		public int ColorIndex;
	}

	public void Init(Config hazardConfig)
	{
		if (Game.Instance.LocalPlayerIndex == hazardConfig.ColorIndex)
		{
			// Hidden
			this.gameObject.layer = LayerMask.NameToLayer("HiddenHazard");
		}

		this.spriteRenderer.sprite = this.gameConfig.GetHazardSprite(hazardConfig.Type);
		this.spriteRenderer.color = this.gameConfig.PlayerColors[hazardConfig.ColorIndex];

		this.Type = hazardConfig.Type;
	}
}
