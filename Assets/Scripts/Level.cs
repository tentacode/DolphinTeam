using UnityEngine;

public class Level : AdvancedMonoBehaviour
{
	[SerializeField]
	private GameConfig gameConfig;

	private void Update()
	{
		this.Tfm.position += this.gameConfig.LevelSpeed * Time.deltaTime * Vector3.up;
	}
}
