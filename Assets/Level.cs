using UnityEngine;

public class Level : MonoBehaviour
{
	[SerializeField]
	private float speed;

	private Transform tfm;

	private void Awake()
	{
		this.tfm = this.GetComponent<Transform>();
	}

	private void Update()
	{
		this.tfm.position += this.speed * Time.deltaTime * Vector3.up;
	}
}
