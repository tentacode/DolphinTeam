using UnityEngine;

[CreateAssetMenu]
public class WaveGroupPattern : ScriptableObject
{
	public int Difficulty;
	public WavePattern[] WavePatterns;
	public bool Disabled;

	public void SpawnWaves(GameConfig gameConfig, int playerCount, float waveSize, ref float waveYPosition)
	{
		if (gameConfig.DebugMode) Debug.LogFormat("WaveGroup = {0}", this.name);

		for (int wavePatternIndex = 0; wavePatternIndex < this.WavePatterns.Length; ++wavePatternIndex)
		{
			// Generate wave
			WavePattern wavePattern = this.WavePatterns[wavePatternIndex];
			wavePattern.SpawnHazards(gameConfig, playerCount, waveYPosition);

			waveYPosition += waveSize;
			++Game.Instance.TotalWaveCount;
		}
	}
}
