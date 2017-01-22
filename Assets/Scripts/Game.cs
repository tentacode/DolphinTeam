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
	[SerializeField]
	private DolphinInput inputManager;
	[SerializeField]
	private AudioSource musicAudioSource;

	private int playerCount;

	public int LocalPlayerIndex { get; private set; }
	public bool IsStarted { get; private set; }
	public int TotalWaveCount { get; set; }

	public Game()
	{
		Game.Instance = this;
	}

	private void Start()
	{
		this.levelScroller.enabled = false;
		this.inputManager.enabled = false;
	}

	public void SetLocalPlayerIndex(int playerIndex)
	{
		Debug.Log("Local player index is " + playerIndex);
		this.LocalPlayerIndex = playerIndex;
	}

	public void SetPlayerCount(int playerCount)
	{
		Debug.Log("Player count is " + playerCount);
		this.playerCount = playerCount;
	}

	public void GenerateLevel(int seed)
	{
		Debug.Log("Seed is " + seed);
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
		this.inputManager.enabled = true;
		this.IsStarted = true;

		if (this.LocalPlayerIndex == 0)
		{
			// Host
			this.musicAudioSource.Play();
		}
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
