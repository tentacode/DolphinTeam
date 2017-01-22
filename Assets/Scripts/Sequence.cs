using UnityEngine;

[CreateAssetMenu]
public class Sequence : ScriptableObject
{
	public string Title;
	public string SubTitle;
	public float TitleTriggerOffset;

	public float WaveSize;

	public virtual void SpawnWaveGroups(GameConfig gameConfig, int playerCount, ref float waveYPosition)
	{
		if (gameConfig.DebugMode) Debug.LogWarningFormat("Sequence = {0}", this.name);

		if (this.Title != null && this.Title.Length > 0)
		{
			GameObject sequenceTriggerGO = GameObject.Instantiate(gameConfig.SequenceTitlePrefab, (waveYPosition + this.TitleTriggerOffset) * Vector3.up, Quaternion.identity);
			sequenceTriggerGO.name = string.Format("Sequence_{0}", this.name);

			SequenceTrigger sequenceTrigger = sequenceTriggerGO.GetComponent<SequenceTrigger>();
			sequenceTrigger.Init(this.Title, this.SubTitle);
		}
	}
}
