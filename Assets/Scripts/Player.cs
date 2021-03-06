﻿using UnityEngine;
using UnityEngine.UI;

public class Player : AdvancedMonoBehaviour
{
	[SerializeField]
	private GameConfig gameConfig;
	[SerializeField]
	private SpriteRenderer spriteRenderer;
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private RectTransform gameOverUI;
	[SerializeField]
	private Text passedWaveCountLabel;
	[SerializeField]
	private RectTransform victoryUI;
	[SerializeField]
	private Text treasuresCountLabel;
	[SerializeField]
    private LifeDisplay lifeDisplay;
    [SerializeField]
    private TreasureDisplay treasureDisplay;
    [SerializeField]
    private AudioSource audioSource, audioMovementLane, audioMovementJump, audioMovementLand, audioSuccess, audioDefeat, audioHit, music;
    [SerializeField]
    private AudioPlayer audioPlayer;
	[SerializeField]
	private string groundedSortingLayerName = "GroundedPlayer";
	[SerializeField]
	private string airborneSortingLayerName = "AirbornePlayer";
	[SerializeField]
	private string jumpAnimTriggerName = "Jump";
	[SerializeField]
	private GameObject godzilla;

	private int columnIndex = 1;
	private Hazard collidingHazard;
	private bool isAirborne;
	private bool isMoving;
	private bool isDead;
	private int heartCount;
	private int treasureCount;
    private bool hasBeenHit;
    private int passedWaveCount;

	private void Start()
	{
        this.hasBeenHit = false;
		this.heartCount = this.gameConfig.InitPlayerHeartCount;
        this.lifeDisplay.UpdateDisplayedLifeCount(this.heartCount);
        this.gameOverUI.Hide();
	}

	/*public void SetLocalPlayerIndex(int playerIndex)
	{
		this.spriteRenderer.color = this.gameConfig.PlayerColors[playerIndex];
	}*/

	private void OnTriggerEnter2D(Collider2D other)
	{
        if (other.CompareTag("Wave") == true )
        {
            if (this.hasBeenHit == false)
            {
                //this.audioPlayer.PlayClip("WaveSuccess");
                audioSuccess.Play();
            }
            this.hasBeenHit = false;
			++this.passedWaveCount;
			if (this.passedWaveCount >= Game.Instance.TotalWaveCount)
			{
				// Victory
				this.victoryUI.Show();
				this.treasuresCountLabel.text = string.Format("TREASURES: {0}", this.treasureCount);

				this.godzilla.SetActive(true);
			}
			return;
        }

		if (gameConfig.DebugMode) Debug.LogFormat("OnTriggerEnter2D {0}", other.name);
		Hazard hazard = other.GetComponent<Hazard>();
		if (hazard == null)
		{
			return;
		}

		this.collidingHazard = hazard;
		if (gameConfig.DebugMode) Debug.LogWarning(this.collidingHazard.Type);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (gameConfig.DebugMode) Debug.LogFormat("OnTriggerExit2D {0}", other.name);
		Hazard hazard = other.GetComponent<Hazard>();
		if (hazard == null)
		{
			return;
		}

		if (this.collidingHazard == hazard)
		{
			this.collidingHazard = null;
		}
	}

	private void Update()
	{
		if (this.isDead)
		{
			return;
		}

		if (!Game.Instance.IsStarted)
		{
			return;
		}

		if (!this.isAirborne && !this.isMoving)
		{
			if (DolphinInput.IsGoingLeft())
			{
				// Move left
				--this.columnIndex;
				this.isMoving = true;

                audioMovementLane.Play();
            }

			if (DolphinInput.IsGoingRight())
			{
				// Move right
				++this.columnIndex;
				this.isMoving = true;

                audioMovementLane.Play();
            }

            if (DolphinInput.IsJumping())
			{
				// Jump
				this.Jump();
			}
		}

		this.columnIndex = Mathf.Clamp(this.columnIndex, 0, this.gameConfig.ColumnCount - 1);

		float xTargetPosition = (float)(this.columnIndex - this.gameConfig.ColumnCount / 2) * this.gameConfig.ColumnWidth;
		float xPosition = Mathf.Lerp(this.Tfm.localPosition.x, xTargetPosition, Time.deltaTime * this.gameConfig.PlayerMoveSpeed);
		this.Tfm.localPosition = new Vector3
		(
			xPosition,
			this.Tfm.localPosition.y,
			this.Tfm.localPosition.z
		);

		if (Mathf.Abs(xPosition - xTargetPosition) <= this.gameConfig.PlayerMoveReachDistance)
		{
			// Move is over
			this.isMoving = false;
		}

		// Check hazard collision
		if (this.collidingHazard != null)
		{
			bool isHit = false;
			bool isCapturingHeart = false;
			bool isCapturingTreasure = false;
			switch (this.collidingHazard.Type)
			{
				// ------------ HIT
				case Hazard.HazardType.Mine:
					isHit = !this.isAirborne;
					break;

				case Hazard.HazardType.Helicopter:
					isHit = this.isAirborne;
					break;

				case Hazard.HazardType.Shark:
					isHit = true;
					break;
				// -------------------

				// ------------ BONUS
				case Hazard.HazardType.GroundHeart:
					isCapturingHeart = !this.isAirborne;
					break;

				case Hazard.HazardType.AirHeart:
					isCapturingHeart = this.isAirborne;
					break;

				case Hazard.HazardType.GroundTreasure:
					isCapturingTreasure = !this.isAirborne;
					break;

				case Hazard.HazardType.AirTreasure:
					isCapturingTreasure = this.isAirborne;
					break;
                // -------------------
			}

			if (isHit)
			{
                //Debug.LogWarning(this.collidingHazard.Type);
                this.hasBeenHit = true;
                this.heartCount = Mathf.Max(this.heartCount - 1, 0);
				this.lifeDisplay.UpdateDisplayedLifeCount(this.heartCount);
				if (this.heartCount == 0)
                {
                    //this.audioPlayer.PlayClip("PlayerHit");
                    audioHit.Play();
					this.passedWaveCountLabel.text = string.Format("PASSED WAVES: {0}", this.passedWaveCount);
					this.gameOverUI.Show();
                    //                    Time.timeScale = 0f;
                    this.isDead = true;
                    music.Stop();
                    audioDefeat.Play();
                    this.collidingHazard.OnPlayerDeadlyCollision();
					GameObject.Destroy(this.gameObject);
                }
                else
                {
                    this.animator.SetTrigger("Hit");
                    //this.audioPlayer.PlayClip("PlayerHit");
                    audioHit.Play();
                    this.collidingHazard.OnPlayerCollision();
                }

				this.collidingHazard.Collider.enabled = false;
				this.collidingHazard = null;
			}

			if (isCapturingHeart)
			{
				this.heartCount = Mathf.Min(this.heartCount + 1, gameConfig.MaxPlayerHeartCount);
				this.lifeDisplay.UpdateDisplayedLifeCount(this.heartCount);
				this.collidingHazard.Collider.enabled = false;
                this.collidingHazard.MakeInvisible();
                this.collidingHazard.OnPlayerCollision();
                this.collidingHazard = null;
			}

			if (isCapturingTreasure)
			{
				++this.treasureCount;
				this.treasureDisplay.UpdateDisplayCount(this.treasureCount);
				this.collidingHazard.Collider.enabled = false;
                this.collidingHazard.MakeInvisible();
                this.collidingHazard.OnPlayerCollision();
                this.collidingHazard = null;
			}
		}
	}

	private void Jump()
	{
		// Going airborne
		this.isAirborne = true;
		this.animator.SetTrigger(this.jumpAnimTriggerName);
		this.spriteRenderer.sortingLayerName = this.airborneSortingLayerName;

        audioMovementJump.Play();
    }

    private void Land()
	{
		// Landing
		this.isAirborne = false;
		this.spriteRenderer.sortingLayerName = this.groundedSortingLayerName;

        audioMovementLand.Play();
    }

}
