using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RandomWavePattern : WavePattern
{
	public Hazard.HazardType[] HazardTypes;
	public bool AuthorizeMineUnderBonus;
	public bool AuthorizeHelicopterOverBonus;
	public bool AuthorizeHelicopterOverMine;

	private Hazard.HazardType[] groundHazards;
	private Hazard.HazardType[] airHazards;
	private List<int> freeColumnIndexes = new List<int>();

	public override void GetHazards(ref List<HazardSpawnConfig> hazardSpawnConfigs, GameConfig gameConfig)
	{
		CollectionTools.Init(ref hazardSpawnConfigs);

		if (this.groundHazards == null)
		{
			this.groundHazards = new Hazard.HazardType[gameConfig.ColumnCount];
		}

		for (int columnIndex = 0; columnIndex < gameConfig.ColumnCount; ++columnIndex)
		{
			this.groundHazards[columnIndex] = Hazard.HazardType.None;
		}

		if (this.airHazards == null)
		{
			this.airHazards = new Hazard.HazardType[gameConfig.ColumnCount];
		}

		for (int columnIndex = 0; columnIndex < gameConfig.ColumnCount; ++columnIndex)
		{
			this.airHazards[columnIndex] = Hazard.HazardType.None;
		}

		for (int hazardIndex = 0; hazardIndex < this.HazardTypes.Length; ++hazardIndex)
		{
			Hazard.HazardType hazardType = this.HazardTypes[hazardIndex];

			int columnIndex = Random.Range(0, gameConfig.ColumnCount);

			// Build free columns list
			this.freeColumnIndexes.Clear();
			for (columnIndex = 0; columnIndex < gameConfig.ColumnCount; ++columnIndex)
			{
				switch (hazardType)
				{
					case Hazard.HazardType.Mine:
					case Hazard.HazardType.GroundHeart:
					case Hazard.HazardType.GroundTreasure:
						this.freeColumnIndexes.AddIfNotAlreadyContained(columnIndex);
						break;

					case Hazard.HazardType.Helicopter:
					case Hazard.HazardType.AirHeart:
					case Hazard.HazardType.AirTreasure:
						this.freeColumnIndexes.AddIfNotAlreadyContained(columnIndex);
						break;

					case Hazard.HazardType.Shark:
						this.groundHazards[columnIndex] = hazardType;
						this.airHazards[columnIndex] = hazardType;
						break;
				}
			}

			switch (hazardType)
			{
				case Hazard.HazardType.Mine:
				case Hazard.HazardType.GroundHeart:
				case Hazard.HazardType.GroundTreasure:
					this.groundHazards[columnIndex] = hazardType;
					break;

				case Hazard.HazardType.Helicopter:
				case Hazard.HazardType.AirHeart:
				case Hazard.HazardType.AirTreasure:
					this.airHazards[columnIndex] = hazardType;
					break;

				case Hazard.HazardType.Shark:
					this.groundHazards[columnIndex] = hazardType;
					this.airHazards[columnIndex] = hazardType;
					break;
			}

			hazardSpawnConfigs.Add(new HazardSpawnConfig() { HazardType = hazardType, ColumnIndex = columnIndex });
		}
	}
}
