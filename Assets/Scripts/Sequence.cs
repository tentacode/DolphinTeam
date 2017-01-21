using UnityEngine;

[CreateAssetMenu]
public class Sequence : ScriptableObject
{
	public float WaveSize;

	public virtual void SpawnWaveGroups(GameConfig gameConfig, int playerCount, ref float waveYPosition)
	{
		if (gameConfig.DebugMode) Debug.LogWarningFormat("Sequence = {0}", this.name);
	}
}
