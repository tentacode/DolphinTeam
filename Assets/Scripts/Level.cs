using UnityEngine;

public class Level : AdvancedMonoBehaviour
{
	[SerializeField]
	private GameConfig gameConfig;

	private float speed;

	public void SetSpeed(float speed)
	{
		this.speed = speed;
	}

	private void Update()
	{
		this.Tfm.position += this.speed * Time.deltaTime * Vector3.up;
	}
}
