using System.Collections.Generic;
using UnityEngine;

public abstract class WavePattern : ScriptableObject
{
	public struct HazardSpawnConfig
	{
		public Hazard.HazardType HazardType;
		public int ColumnIndex;
	}

	public abstract void GetHazards(ref List<HazardSpawnConfig> hazardSpawnConfigs, GameConfig gameConfig);
}
