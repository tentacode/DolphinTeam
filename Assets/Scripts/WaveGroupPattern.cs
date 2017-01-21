using UnityEngine;

[CreateAssetMenu]
public class WaveGroupPattern : ScriptableObject
{
	public int Difficulty;
	public WavePattern[] WavePatterns;

	public void SpawnWaves(GameConfig gameConfig, int playerCount, float waveSize, ref float waveYPosition)
	{
		//Debug.LogFormat("WaveGroup > {0}",  difficultyWaveGroupPattern.name);

		for (int wavePatternIndex = 0; wavePatternIndex < this.WavePatterns.Length; ++wavePatternIndex)
		{
			// Generate wave
			WavePattern wavePattern = this.WavePatterns[wavePatternIndex];

			GameObject waveGO = GameObject.Instantiate(gameConfig.WavePrefab, waveYPosition * Vector3.up + gameConfig.WavePrefab.transform.position.z * Vector3.forward, Quaternion.identity);
			waveGO.name = wavePattern.name;

			wavePattern.SpawnHazards(gameConfig, playerCount, waveYPosition);

			waveYPosition += waveSize;
		}
	}
}
