using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Chapter : Sequence
{
	public int WaveGroupCount;
	public AnimationCurve MinDifficultyCurve;
	public AnimationCurve MaxDifficultyCurve;

	private List<WaveGroupPattern> difficultyWaveGroupPatterns = new List<WaveGroupPattern>();

	public override void SpawnWaveGroups(GameConfig gameConfig, int playerCount, ref float waveYPosition)
	{
		base.SpawnWaveGroups(gameConfig, playerCount, ref waveYPosition);

		bool noHazardHiding = false;

		// Generate pattern groups
		for (int waveGroupPatternNumber = 1; waveGroupPatternNumber <= this.WaveGroupCount; ++waveGroupPatternNumber)
		{
			// Compute pattern group difficulty
			float difficultyTime = (float)waveGroupPatternNumber / (float)this.WaveGroupCount;
			float minDifficulty = this.MinDifficultyCurve.Evaluate(difficultyTime);
			float maxDifficulty = this.MaxDifficultyCurve.Evaluate(difficultyTime);
			//if (gameConfig.DebugMode) Debug.LogFormat("minDifficulty = {0}", minDifficulty);
			//if (gameConfig.DebugMode) Debug.LogFormat("maxDifficulty = {0}", maxDifficulty);

			// Filter pattern groups for difficulty
			this.difficultyWaveGroupPatterns.Clear();
			for (int patternGroupIndex = 0; patternGroupIndex < gameConfig.WaveGroupPatterns.Length; ++patternGroupIndex)
			{
				WaveGroupPattern waveGroupPattern = gameConfig.WaveGroupPatterns[patternGroupIndex];
				if (!waveGroupPattern.Disabled && waveGroupPattern.Difficulty >= minDifficulty && waveGroupPattern.Difficulty <= maxDifficulty)
				{
					this.difficultyWaveGroupPatterns.Add(waveGroupPattern);
				}
			}
			if (gameConfig.DebugMode) Debug.LogFormat("difficultyWaveGroupPatterns.Count = {0}", this.difficultyWaveGroupPatterns.Count);

			if (this.difficultyWaveGroupPatterns.Count < 1)
			{
				Debug.LogErrorFormat("No pattern group for difficulty between {0} and {1}!", minDifficulty, maxDifficulty);
				break;
			}

			// Generate wave group
			WaveGroupPattern difficultyWaveGroupPattern = this.difficultyWaveGroupPatterns[Random.Range(0, this.difficultyWaveGroupPatterns.Count)];
			difficultyWaveGroupPattern.SpawnWaves(gameConfig, playerCount, this.WaveSize, ref waveYPosition);
		}
	}
}
