using UnityEngine;

public class Step : MonoBehaviour
{
	[SerializeField]
	private GameConfig gameConfig;

	public void Init(Hazard.Config[] hazardConfigs)
	{
		for (int columnIndex = 0; columnIndex < hazardConfigs.Length; columnIndex++)
		{
			Hazard.Config hazardConfig = hazardConfigs[columnIndex];
			if (hazardConfig == null)
			{
				continue;
			}

			GameObject hazardGO = GameObject.Instantiate(this.gameConfig.HazardPrefab, Vector3.zero, Quaternion.identity);

			hazardGO.transform.parent = this.transform;
			hazardGO.transform.localPosition = (columnIndex - (this.gameConfig.ColumnCount / 2)) * this.gameConfig.MoveUnit * Vector3.right;

			Hazard hazard = hazardGO.GetComponent<Hazard>();
			hazard.Init(hazardConfig);
		}
	}
}
