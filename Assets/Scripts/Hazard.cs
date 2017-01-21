using UnityEngine;

public class Hazard : AdvancedMonoBehaviour
{
	[SerializeField]
	private GameConfig gameConfig;
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	public HazardType Type { get; private set; }
    public BoxCollider2D Collider { get; private set; }

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

	public void Init(HazardType type, int colorIndex)
	{
		this.Type = type;
        this.Collider = GetComponent<BoxCollider2D>();

        if (Game.Instance.LocalPlayerIndex == colorIndex)
		{
			// Hidden
			this.gameObject.layer = LayerMask.NameToLayer("HiddenHazard");
		}

		this.spriteRenderer.sprite = this.gameConfig.GetHazardSprite(this.Type);
		this.spriteRenderer.color = this.gameConfig.PlayerColors[colorIndex];
	}
}
