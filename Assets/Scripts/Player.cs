using System.Collections;
using UnityEngine;

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
    private LifeDisplay lifeDisplay;
    [SerializeField]
    private TreasureDisplay treasureDisplay;

    private int hPosition = 0;
	private Hazard collidingHazard;
	private bool isAirborne;
	private bool isDead;
	private int heartCount;
	private int treasureCount;

	private void Start()
	{
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
		//Debug.LogFormat("OnTriggerEnter2D {0}", other.name);
		Hazard hazard = other.GetComponent<Hazard>();
		if (hazard == null)
		{
			return;
		}

		this.collidingHazard = hazard;
		//Debug.LogWarning(this.collidingHazard.Type);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		//Debug.LogFormat("OnTriggerExit2D {0}", other.name);
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

		if (!this.isAirborne)
		{
			if (DolphinInput.IsGoingLeft())
			{
				// Move left
				--this.hPosition;
			}

			if (DolphinInput.IsGoingRight())
			{
				// Move right
				++this.hPosition;
			}

			if (DolphinInput.IsJumping())
			{
				// Jump
				this.StartCoroutine(this.Jump());
			}
		}

		this.hPosition = Mathf.Clamp(this.hPosition, -(this.gameConfig.ColumnCount / 2), this.gameConfig.ColumnCount / 2);

		this.Tfm.localPosition = new Vector3
		(
			(float)this.hPosition * this.gameConfig.MoveUnit,
			this.Tfm.localPosition.y,
			this.Tfm.localPosition.z
		);

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
				Debug.LogError(this.collidingHazard.Type);
				this.heartCount = Mathf.Max(this.heartCount - 1, 0);
				this.lifeDisplay.UpdateDisplayedLifeCount(this.heartCount);
                if (this.heartCount == 0)
                {
                    this.gameOverUI.Show();
                    //                    Time.timeScale = 0f;
                    this.isDead = true;
                    this.collidingHazard.OnPlayerDeadlyCollision();
                    GameObject.Destroy(this.gameObject);
                }
                else
                {
                    this.animator.SetTrigger("Hit");
                    this.collidingHazard.OnPlayerCollision();
                }
                this.collidingHazard.Collider.enabled = false;
			}

			if (isCapturingHeart)
			{
				this.heartCount = Mathf.Min(this.heartCount + 1, gameConfig.MaxPlayerHeartCount);
				this.lifeDisplay.UpdateDisplayedLifeCount(this.heartCount);
				this.collidingHazard.Collider.enabled = false;
			}

			if (isCapturingTreasure)
			{
				++this.treasureCount;
				this.treasureDisplay.UpdateDisplayCount(this.treasureCount);
				this.collidingHazard.Collider.enabled = false;
			}
		}
	}

	private IEnumerator Jump()
	{
		this.isAirborne = true;
		this.animator.SetBool("IsAirborne", this.isAirborne);

		yield return new WaitForSeconds(this.gameConfig.JumpDuration);

		this.isAirborne = false;
		this.animator.SetBool("IsAirborne", this.isAirborne);
	}

}
