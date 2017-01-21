using UnityEngine;

[CreateAssetMenu]
public class Sequence : ScriptableObject
{
	public float WaveSize;

	public virtual void SpawnWaveGroups(GameConfig gameConfig, int playerCount, ref float waveYPosition)
	{
		//Debug.LogWarningFormat("Sequence > {0}", sequence.name);
	}
}
