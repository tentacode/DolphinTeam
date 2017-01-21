using UnityEngine;

public class Step : AdvancedMonoBehaviour
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
			Hazard hazard = hazardGO.GetComponent<Hazard>();

			hazard.Tfm.parent = this.Tfm;
			hazard.Tfm.localPosition = (columnIndex - (this.gameConfig.ColumnCount / 2)) * this.gameConfig.MoveUnit * Vector3.right;

			hazard.Init(hazardConfig);
		}
	}
}
