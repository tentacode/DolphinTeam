using UnityEngine;

public class SequenceTrigger : AdvancedMonoBehaviour
{
	private string title;
	private string subTitle;

	public void Init(string title, string subTitle)
	{
		this.title = title;
		this.subTitle = subTitle;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Player player = other.GetComponent<Player>();
		if (player == null)
		{
			return;
		}

		Game.Instance.DisplaySequenceTitle(this.title, this.subTitle);
	}
}
