using UnityEngine;

public class Level : MonoBehaviour
{
	[SerializeField]
	private GameConfig gameConfig;

	private Transform tfm;

	private void Awake()
	{
		this.tfm = this.GetComponent<Transform>();
	}

	private void Update()
	{
		this.tfm.position += this.gameConfig.LevelSpeed * Time.deltaTime * Vector3.up;
	}
}
