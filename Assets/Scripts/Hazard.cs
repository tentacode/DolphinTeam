using UnityEngine;

public class Hazard : AdvancedMonoBehaviour
{
	[SerializeField]
	private GameConfig gameConfig;
	[SerializeField]
	private SpriteRenderer spriteRenderer;
    [SerializeField]
    private AudioPlayer audioPlayer;
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

	public void Init(HazardType type, int playerIndex, bool noHazardHiding)
	{
		this.Type = type;
        this.Collider = GetComponent<BoxCollider2D>();

        this.audioPlayer.SetAudioIdle(this.gameConfig.GetHazardAudio(this.Type.ToString() + "Idle"));
        this.audioPlayer.SetAudioTouched(this.gameConfig.GetHazardAudio(this.Type.ToString() + "Touched"));

        if (!noHazardHiding && Game.Instance.LocalPlayerIndex != playerIndex)
		{
			// Hidden
			this.MakeInvisible();
		}
        else
        {
            this.audioPlayer.PlayIdle();
        }

        this.animator.runtimeAnimatorController = this.gameConfig.GetHazardAnimator(this.Type);
        if (this.animator.runtimeAnimatorController == null)
        {
            this.spriteRenderer.sprite = this.gameConfig.GetHazardSprite(this.Type);
        }

        //this.spriteRenderer.color = noHazardHiding ? this.gameConfig.NeutralColor : this.gameConfig.PlayerColors[colorIndex];

        switch (this.Type)
		{
			case HazardType.Helicopter:
			case HazardType.AirHeart:
			case HazardType.AirTreasure:
				// Air
				Vector3 visualPosition = this.spriteRenderer.transform.localPosition;
				visualPosition.z = -2;
				this.spriteRenderer.transform.localPosition = visualPosition;
				break;
		}
	}

	public void OnPlayerDeadlyCollision()
    {
		if (this.animator.runtimeAnimatorController == null)
		{
			return;
		}
        this.animator.SetTrigger("Dead");
        this.audioPlayer.PlayTouched();
        MakeVisible();
    }

    public void OnPlayerCollision()
    {
		if (this.animator.runtimeAnimatorController == null)
		{
			return;
		}
		this.animator.SetTrigger("Touched");
        this.audioPlayer.PlayTouched();
        MakeVisible();
    }

    private void MakeVisible()
    {
        this.spriteRenderer.gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void MakeInvisible()
    {
        this.spriteRenderer.gameObject.layer = LayerMask.NameToLayer("HiddenHazard");
    }
}
