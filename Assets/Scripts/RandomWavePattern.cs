using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RandomWavePattern : WavePattern
{
	public Hazard.HazardType[] HazardTypes;
	public bool AuthorizeBonusOverMine;
	public bool AuthorizeHelicopterOverBonus;
	public bool AuthorizeHelicopterOverMine;
	public bool AuthorizeBonusOverBonus;

	private Hazard.HazardType[] groundHazards = null;
	private Hazard.HazardType[] airHazards = null;
	private List<int> freeColumnIndexes = new List<int>();

	public override void GetHazards(ref List<HazardSpawnConfig> hazardSpawnConfigs, GameConfig gameConfig)
	{
		CollectionTools.Init(ref hazardSpawnConfigs);

		// Reset wave slots
		if (this.groundHazards == null || this.groundHazards.Length != gameConfig.ColumnCount)
		{
            this.groundHazards = new Hazard.HazardType[gameConfig.ColumnCount];
		}

		if (this.airHazards == null || this.airHazards.Length != gameConfig.ColumnCount)
		{
			this.airHazards = new Hazard.HazardType[gameConfig.ColumnCount];
		}

		for (int columnIndex = 0; columnIndex < gameConfig.ColumnCount; ++columnIndex)
		{
            this.groundHazards[columnIndex] = Hazard.HazardType.None;
			this.airHazards[columnIndex] = Hazard.HazardType.None;
		}

		for (int hazardIndex = 0; hazardIndex < this.HazardTypes.Length; ++hazardIndex)
		{
			Hazard.HazardType hazardType = this.HazardTypes[hazardIndex];

			// Scan for free columns
			this.freeColumnIndexes.Clear();
			for (int columnIndex = 0; columnIndex < gameConfig.ColumnCount; ++columnIndex)
			{
				bool freeColumn = false;
				switch (hazardType)
				{
					case Hazard.HazardType.Mine:
						freeColumn =
						(
							// Nothing on ground
							groundHazards[columnIndex] == Hazard.HazardType.None &&
							(
								// Nothing in the air
								airHazards[columnIndex] == Hazard.HazardType.None ||
								// Helicopter over but authorized
								(airHazards[columnIndex] == Hazard.HazardType.Helicopter && this.AuthorizeHelicopterOverMine) ||
								// Bonus over but authorized
								((airHazards[columnIndex] == Hazard.HazardType.AirTreasure || airHazards[columnIndex] == Hazard.HazardType.AirHeart) && this.AuthorizeBonusOverMine)
							)
						);
						break;

					case Hazard.HazardType.Helicopter:
						freeColumn =
						(
							// Nothing in the air
							airHazards[columnIndex] == Hazard.HazardType.None &&
							(
								// Nothing on the ground
								groundHazards[columnIndex] == Hazard.HazardType.None ||
								// Mine under but authorized
								(groundHazards[columnIndex] == Hazard.HazardType.Mine && this.AuthorizeHelicopterOverMine) ||
								// Bonus under but authorized
								((groundHazards[columnIndex] == Hazard.HazardType.AirTreasure || groundHazards[columnIndex] == Hazard.HazardType.AirHeart) && this.AuthorizeHelicopterOverBonus)
							)
						);
						break;

					case Hazard.HazardType.Shark:
						freeColumn =
						(
							// Nothing on the ground
							groundHazards[columnIndex] == Hazard.HazardType.None &&
							// Nothing in the air
							airHazards[columnIndex] == Hazard.HazardType.None
						);
						break;

					case Hazard.HazardType.GroundHeart:
					case Hazard.HazardType.GroundTreasure:
						freeColumn =
						(
							// Nothing on the ground
							groundHazards[columnIndex] == Hazard.HazardType.None &&
							(
								// Nothing in the air
								airHazards[columnIndex] == Hazard.HazardType.None ||
								// Helicopter over but authorized
								(airHazards[columnIndex] == Hazard.HazardType.Helicopter && this.AuthorizeHelicopterOverBonus) ||
								// Bonus over but authorized
								((airHazards[columnIndex] == Hazard.HazardType.AirTreasure || airHazards[columnIndex] == Hazard.HazardType.AirHeart) && this.AuthorizeBonusOverBonus)
							)
						);
						break;

					case Hazard.HazardType.AirHeart:
					case Hazard.HazardType.AirTreasure:
						freeColumn =
						(
							// Nothing in the air
							airHazards[columnIndex] == Hazard.HazardType.None &&
							(
								// Nothing on the ground
								groundHazards[columnIndex] == Hazard.HazardType.None ||
								// Mine under but authorized
								(groundHazards[columnIndex] == Hazard.HazardType.Helicopter && this.AuthorizeBonusOverMine) ||
								// Bonus under but authorized
								((groundHazards[columnIndex] == Hazard.HazardType.AirTreasure || groundHazards[columnIndex] == Hazard.HazardType.AirHeart) && this.AuthorizeBonusOverBonus)
							)
						);
						break;
				}

				if (freeColumn)
				{
					this.freeColumnIndexes.Add(columnIndex);
				}
			}

            // Place hazard
            if (this.freeColumnIndexes.Count < 1)
            {
                Debug.LogErrorFormat("{0} RandomWavePattern > Unable to spawn {1}, no free column!", this.name, hazardType);
            }
            else
            {
                int freeColumnIndex = this.freeColumnIndexes[Random.Range(0, this.freeColumnIndexes.Count)];
                switch (hazardType)
                {
                    case Hazard.HazardType.Mine:
                    case Hazard.HazardType.Shark:
                    case Hazard.HazardType.GroundHeart:
                    case Hazard.HazardType.GroundTreasure:
                        this.groundHazards[freeColumnIndex] = hazardType;
                        break;

                    case Hazard.HazardType.Helicopter:
                    case Hazard.HazardType.AirHeart:
                    case Hazard.HazardType.AirTreasure:
                        this.airHazards[freeColumnIndex] = hazardType;
                        break;
                }

                hazardSpawnConfigs.Add(new HazardSpawnConfig() { HazardType = hazardType, ColumnIndex = freeColumnIndex });
            }
        }
	}
}
