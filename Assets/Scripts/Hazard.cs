using System;
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
	[SerializeField]
	private string groundSortingLayerName = "GroundHazard";
	[SerializeField]
	private string airSortingLayerName = "AirHazard";
	[SerializeField]
	private float airYOffset;
    [SerializeField]
    private AudioSource audioTouched, audioIdle;

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

        //this.SetAudioIdle(this.gameConfig.GetHazardAudio(this.Type.ToString() + "Idle"));
        //this.SetAudioTouched(this.gameConfig.GetHazardAudio(this.Type.ToString() + "Touched"));

        if (!noHazardHiding && Game.Instance.LocalPlayerIndex != playerIndex)
		{
			// Hidden
			this.MakeInvisible();
		}
        else
        {
            //this.audioPlayer.PlayIdle();
            this.audioIdle.Play();
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
				this.spriteRenderer.sortingLayerName = this.airSortingLayerName;
				this.Tfm.position += this.airYOffset * Vector3.up;                
                break;

			default:
				// Ground
				this.spriteRenderer.sortingLayerName = this.groundSortingLayerName;           
				break;
		}

        switch (Type) {
            case HazardType.None:
                break;
            case HazardType.Mine:
            case HazardType.Helicopter:
            case HazardType.Shark:
                this.SetAudioIdle(this.gameConfig.GetHazardAudio(this.Type.ToString() + "Idle"));
                this.SetAudioTouched(this.gameConfig.GetHazardAudio(this.Type.ToString() + "Touched"));
                break;

            case HazardType.GroundHeart:
            case HazardType.AirHeart:
            case HazardType.GroundTreasure:
            case HazardType.AirTreasure:
                this.SetAudioTouched(this.gameConfig.GetHazardAudio("pickup"));
                break;
            default:
                break;
        }
    }

    private void SetAudioTouched(AudioClip audioClip) {
        audioTouched.clip = audioClip;
    }

    private void SetAudioIdle(AudioClip audioClip) {
        audioIdle.clip = audioClip;
    }

    public void OnPlayerDeadlyCollision()
    {
		if (this.animator.runtimeAnimatorController == null)
		{
			return;
		}
        this.animator.SetTrigger("Dead");
        //this.audioPlayer.PlayTouched();
        this.audioTouched.Play();
        MakeVisible();
    }

    public void OnPlayerCollision()
    {
		if (this.animator.runtimeAnimatorController == null)
		{
			return;
		}
		this.animator.SetTrigger("Touched");
        //this.audioPlayer.PlayTouched();
        this.audioTouched.Play();
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
