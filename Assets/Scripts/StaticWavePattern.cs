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

		hazardSpawnConfigs.Add(new HazardSpawnConfig() { HazardType = this.GroundLeft, ColumnIndex = 0 });
		hazardSpawnConfigs.Add(new HazardSpawnConfig() { HazardType = this.GroundMiddle, ColumnIndex = 1 });
		hazardSpawnConfigs.Add(new HazardSpawnConfig() { HazardType = this.GroundRight, ColumnIndex = 2 });
	}
}
