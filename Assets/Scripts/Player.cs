using System.Collections;
using UnityEngine;

public class Player : AdvancedMonoBehaviour
{
	public static Player Instance { get; private set; }

	[SerializeField]
	private GameConfig gameConfig;
	[SerializeField]
	private SpriteRenderer spriteRenderer;
	[SerializeField]
	private float jumpDuration;
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
	private bool isAirborn;
	private bool isDead;
	private int heartCount;
	private int treasureCount;

	public Player()
	{
		Player.Instance = this;
	}

	private void Start()
	{
		this.spriteRenderer.color = this.gameConfig.PlayerColors[Game.Instance.LocalPlayerIndex];
		this.heartCount = this.gameConfig.InitPlayerHeartCount;
        this.lifeDisplay.UpdateDisplayedLifeCount(this.heartCount);
        this.gameOverUI.Hide();
	}

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

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			// Move left
			--this.hPosition;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			// Move right
			++this.hPosition;
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			// Jump
			this.StartCoroutine(this.Jump());
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
			bool hit = false;
			switch (this.collidingHazard.Type)
			{
				case Hazard.HazardType.Mine:
					hit = !this.isAirborn;
					break;

				case Hazard.HazardType.Helicopter:
					hit = this.isAirborn;
					break;

				case Hazard.HazardType.Shark:
					hit = true;
					break;


                    // ------------ BONUS
                case Hazard.HazardType.Heart:
                    this.heartCount = Mathf.Min(this.heartCount + 1, gameConfig.MaxPlayerHeartCount);
                    this.lifeDisplay.UpdateDisplayedLifeCount(this.heartCount);
                    this.collidingHazard.Collider.enabled = false;
					break;

				case Hazard.HazardType.Treasure:
					++this.treasureCount;
                    this.treasureDisplay.UpdateDisplayCount(this.treasureCount);
                    this.collidingHazard.Collider.enabled = false;
                    break;
                    // -------------------
			}

			if (hit)
			{
                Debug.LogError(this.collidingHazard.Type);
				this.animator.SetTrigger("Hit");
                this.heartCount = Mathf.Max(this.heartCount - 1, 0);
                this.lifeDisplay.UpdateDisplayedLifeCount(this.heartCount);
                if (this.heartCount == 0)
                {
                    this.gameOverUI.Show();
                    Time.timeScale = 0f;
                    this.isDead = true;
                }
                this.collidingHazard.Collider.enabled = false;
            }
        }
	}

	private IEnumerator Jump()
	{
		this.isAirborn = true;
		this.animator.SetBool("IsAirborn", this.isAirborn);

		yield return new WaitForSeconds(this.jumpDuration);

		this.isAirborn = false;
		this.animator.SetBool("IsAirborn", this.isAirborn);
	}
}
