using System.Collections.Generic;
using UnityEngine;

public class WavePattern : ScriptableObject
{
	public struct HazardSpawnConfig
	{
		public Hazard.HazardType HazardType;
		public int ColumnIndex;
	}

	public virtual void GetHazards(ref List<HazardSpawnConfig> hazardSpawnConfigs, GameConfig gameConfig) { }
}
