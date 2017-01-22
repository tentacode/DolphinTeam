using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	public static Game Instance { get; private set; }

	[SerializeField]
	private GameConfig gameConfig;
	[SerializeField]
	private Camera debugCamera;
	[SerializeField]
	private Text sequenceTitleLabel;
	[SerializeField]
	private Text sequenceSubTitleLabel;
	[SerializeField]
	private Animator sequenceTitleAnimator;
	[SerializeField]
	private string sequenceTitleDisplayTriggerName = "Display";
	[SerializeField]
	private LevelScroller levelScroller;

	private int playerCount;

	public int LocalPlayerIndex { get; private set; }

	public Game()
	{
		Game.Instance = this;
	}

	public void SetLocalPlayerIndex(int playerIndex)
	{
		this.LocalPlayerIndex = playerIndex;
	}

	public void SetPlayerCount(int playerCount)
	{
		this.playerCount = playerCount;
	}

	public void GenerateLevel(int seed)
	{
		Random.InitState(seed);

		float waveYPosition = this.gameConfig.FirstWaveOffset;

		// Generate sequence
		for (int sequenceIndex = 0; sequenceIndex < this.gameConfig.Sequences.Length; ++sequenceIndex)
		{
			Sequence sequence = this.gameConfig.Sequences[sequenceIndex];
			sequence.SpawnWaveGroups(this.gameConfig, this.playerCount, ref waveYPosition);
		}
	}

	public void StartGame()
	{
		this.levelScroller.enabled = true;
	}

	public void Restart()
	{
		SceneManager.LoadScene(0);
		Time.timeScale = 1f;
	}

	public void DisplaySequenceTitle(string title, string subTitle)
	{
		this.sequenceTitleLabel.text = title;
		this.sequenceSubTitleLabel.text = subTitle;
		this.sequenceTitleAnimator.SetTrigger(this.sequenceTitleDisplayTriggerName);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			// Toggle debug mode
			this.debugCamera.enabled = !this.debugCamera.enabled;
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			this.Restart();
		}

		if (Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			Time.timeScale *= 2f;
		}
		else if (Input.GetKeyDown(KeyCode.KeypadMinus))
		{
			Time.timeScale *= 0.5f;
		}
		else if (Input.GetKeyDown(KeyCode.KeypadEquals))
		{
			Time.timeScale = 1f;
		}
	}
}
