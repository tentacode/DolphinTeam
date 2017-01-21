using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StaticWavePattern : WavePattern
{
	public Hazard.HazardType GroundLeft;
	public Hazard.HazardType GroundMiddle;
	public Hazard.HazardType GroundRight;
	public Hazard.HazardType AirLeft;
	public Hazard.HazardType AirMiddle;
	public Hazard.HazardType AirRight;

	public override void GetHazards(ref List<HazardSpawnConfig> hazardSpawnConfigs, GameConfig gameConfig)
	{
		CollectionTools.Init(ref hazardSpawnConfigs);

		this.AddHazardSpawn(ref hazardSpawnConfigs, this.GroundLeft, 0);
		this.AddHazardSpawn(ref hazardSpawnConfigs, this.GroundMiddle, 1);
		this.AddHazardSpawn(ref hazardSpawnConfigs, this.GroundRight, 2);
		this.AddHazardSpawn(ref hazardSpawnConfigs, this.AirLeft, 0);
		this.AddHazardSpawn(ref hazardSpawnConfigs, this.AirMiddle, 1);
		this.AddHazardSpawn(ref hazardSpawnConfigs, this.AirRight, 2);
	}

	private void AddHazardSpawn(ref List<HazardSpawnConfig> hazardSpawnConfigs, Hazard.HazardType hazardType, int columnIndex)
	{
		if (hazardType == Hazard.HazardType.None)
		{
			return;
		}

		hazardSpawnConfigs.Add(new HazardSpawnConfig()
		{
			HazardType = hazardType,
			ColumnIndex = columnIndex
		});
	}
}
