using UnityEngine;

[CreateAssetMenu]
public class Transition : Sequence
{
	public WavePattern[] WavePatterns;

	public override void SpawnWaveGroups(GameConfig gameConfig, int playerCount, ref float waveYPosition)
	{
		base.SpawnWaveGroups(gameConfig, playerCount, ref waveYPosition);

		WavePattern wavePattern = this.WavePatterns[Random.Range(0, this.WavePatterns.Length)];
		wavePattern.SpawnHazards(gameConfig, playerCount, waveYPosition, true);

		waveYPosition += this.WaveSize;
	}
}
