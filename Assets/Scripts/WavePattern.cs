using System.Collections.Generic;
using UnityEngine;

public class WavePattern : ScriptableObject
{
	public struct HazardSpawnConfig
	{
		public Hazard.HazardType HazardType;
		public int ColumnIndex;
	}

	private List<WavePattern.HazardSpawnConfig> waveHazardSpawnConfigs;

	public virtual void GetHazards(ref List<HazardSpawnConfig> hazardSpawnConfigs, GameConfig gameConfig) { }

	public void SpawnHazards(GameConfig gameConfig, int playerCount, float waveYPosition, bool noHazardHiding = false)
	{
		//Debug.LogFormat("Wave > {0}", wavePattern.name);

		this.GetHazards(ref this.waveHazardSpawnConfigs, gameConfig);

		for (int hazardIndex = 0; hazardIndex < this.waveHazardSpawnConfigs.Count; ++hazardIndex)
		{
			WavePattern.HazardSpawnConfig hazardSpawnConfig = this.waveHazardSpawnConfigs[hazardIndex];

			float hazardXPosition = (hazardSpawnConfig.ColumnIndex - (gameConfig.ColumnCount / 2)) * gameConfig.MoveUnit;
			int playerIndex = Random.Range(0, playerCount);

			GameObject hazardGO = GameObject.Instantiate(gameConfig.HazardPrefab, hazardXPosition * Vector3.right + waveYPosition * Vector3.up + gameConfig.HazardPrefab.transform.position.z * Vector3.forward, Quaternion.identity);
			hazardGO.name = hazardSpawnConfig.HazardType.ToString();

			Hazard hazard = hazardGO.GetComponent<Hazard>();
			hazard.Init(hazardSpawnConfig.HazardType, playerIndex, noHazardHiding);
		}
	}
}
