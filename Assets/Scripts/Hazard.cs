using UnityEngine;

public class Hazard : AdvancedMonoBehaviour
{
	[SerializeField]
	private GameConfig gameConfig;
	[SerializeField]
	private SpriteRenderer spriteRenderer;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private Animator animator;

	public HazardType Type { get; private set; }
    public BoxCollider2D Collider { get; private set; }

	[System.Serializable]
	public enum HazardType
	{
		None,
		Mine,
		Helicopter,
		Shark,
		GroundHeart,
		AirHeart,
		GroundTreasure,
		AirTreasure,
	}

	public void Init(HazardType type, int colorIndex, bool noHazardHiding)
	{
		this.Type = type;
        this.Collider = GetComponent<BoxCollider2D>();

        if (!noHazardHiding && Game.Instance.LocalPlayerIndex == colorIndex)
		{
			// Hidden
			this.gameObject.layer = LayerMask.NameToLayer("HiddenHazard");
		}

		this.spriteRenderer.sprite = this.gameConfig.GetHazardSprite(this.Type);
        this.audioSource.clip = this.gameConfig.GetHazardAudio(this.Type);
        this.animator.runtimeAnimatorController = this.gameConfig.GetHazardAnimator(this.Type);
		//this.spriteRenderer.color = noHazardHiding ? this.gameConfig.NeutralColor : this.gameConfig.PlayerColors[colorIndex];
	}

    public void OnPlayerDeadlyCollision()
    {
		if (this.animator.runtimeAnimatorController == null)
		{
			return;
		}
        this.audioSource.Play();
        this.animator.SetTrigger("Dead");
    }

    public void OnPlayerCollision()
    {
		if (this.animator.runtimeAnimatorController == null)
		{
			return;
		}
        this.audioSource.Play();
		this.animator.SetTrigger("Touched");
    }
}
