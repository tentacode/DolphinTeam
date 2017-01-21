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

	private int hPosition = 0;
	private Hazard collidingHazard;

	public bool AirBorn { get; private set; }

	public Player()
	{
		Player.Instance = this;
	}

	private void Start()
	{
		this.spriteRenderer.color = this.gameConfig.Colors[Game.Instance.LocalPlayerIndex];
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.LogFormat("OnTriggerEnter2D {0}", other.name);
		Hazard hazard = other.GetComponent<Hazard>();
		if (hazard == null)
		{
			return;
		}

		this.collidingHazard = hazard;
		Debug.LogWarning(this.collidingHazard.Type);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		Debug.LogFormat("OnTriggerExit2D {0}", other.name);
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
		if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			// Move left
			--this.hPosition;
		}
		else if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			// Move right
			++this.hPosition;
		}
		else if (Input.GetKeyUp(KeyCode.UpArrow))
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
			bool death = false;
			switch (this.collidingHazard.Type)
			{
				case Hazard.HazardType.Mine:
					death = !this.AirBorn;
					break;

				case Hazard.HazardType.Helicopter:
					death = this.AirBorn;
					break;

				case Hazard.HazardType.Shark:
					death = true;
					break;
			}

			if (death)
			{
				Debug.LogError("DEATH MOTHER FUCKER!!!");
			}
		}
	}

	private IEnumerator Jump()
	{
		this.AirBorn = true;
		this.animator.SetTrigger("Jump");

		yield return new WaitForSeconds(this.jumpDuration);

		this.AirBorn = false;
		this.animator.SetTrigger("Land");
	}
}
