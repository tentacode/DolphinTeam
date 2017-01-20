using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private float moveUnit;

	private Transform tfm;
	private int hPosition = 0;

	private void Awake()
	{
		this.tfm = this.GetComponent<Transform>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log(other.name);
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			--this.hPosition;
		}
		else if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			++this.hPosition;
		}

		this.hPosition = Mathf.Clamp(this.hPosition, -1, 1);

		this.tfm.localPosition = new Vector3
		(
			(float)this.hPosition * this.moveUnit,
			this.tfm.localPosition.y,
			this.tfm.localPosition.z
		);
	}
}
