using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	public static Game Instance { get; private set; }

	[SerializeField]
	private GameConfig gameConfig;
	[SerializeField]
	private int seed;
	[SerializeField]
	private int playerCount;
	[SerializeField]
	private Camera debugCamera;
	[SerializeField]
	private int localPlayerIndex;

	private List<WaveGroupPattern> difficultyWaveGroupPatterns = new List<WaveGroupPattern>();

	public int LocalPlayerIndex { get { return this.localPlayerIndex; } }

	public Game()
	{
		Game.Instance = this;
	}

	private void Start()
	{
		Time.timeScale = 1f;
		Random.InitState(this.seed);

		int globalWaveNumber = 1;

		// Generate sequence
		for (int sequenceIndex = 1; sequenceIndex <= this.gameConfig.Sequences.Length; ++sequenceIndex)
		{
			Sequence sequence = this.gameConfig.Sequences[sequenceIndex];

			// Generate pattern groups
			for (int waveGroupPatternNumber = 1; waveGroupPatternNumber <= sequence.WaveGroupCount; ++waveGroupPatternNumber)
			{
				// Compute pattern group difficulty
				float difficultyTime = (float)waveGroupPatternNumber / (float)sequence.WaveGroupCount;
				float minDifficulty = sequence.MinDifficultyCurve.Evaluate(difficultyTime);
				float maxDifficulty = sequence.MaxDifficultyCurve.Evaluate(difficultyTime);

				// Filter pattern groups for difficulty
				this.difficultyWaveGroupPatterns.Clear();
				for (int patternGroupIndex = 0; patternGroupIndex < this.gameConfig.WaveGroupPatterns.Length; ++patternGroupIndex)
				{
					WaveGroupPattern waveGroupPattern = this.gameConfig.WaveGroupPatterns[patternGroupIndex];
					if (waveGroupPattern.Difficulty >= minDifficulty && waveGroupPattern.Difficulty <= maxDifficulty)
					{
						this.difficultyWaveGroupPatterns.Add(waveGroupPattern);
					}
				}

				if (this.difficultyWaveGroupPatterns.Count < 1)
				{
					Debug.LogErrorFormat("No pattern group for difficulty between {0} and {1}!", minDifficulty, maxDifficulty);
					break;
				}

				// Generate wave group
				WaveGroupPattern difficultyWaveGroupPattern = this.difficultyWaveGroupPatterns[Random.Range(0, this.difficultyWaveGroupPatterns.Count)];
				for (int patternIndex = 0; patternIndex < difficultyWaveGroupPattern.WavePatterns.Length; ++patternIndex)
				{
					// Generate wave
					WavePattern wavePattern = difficultyWaveGroupPattern.WavePatterns[patternIndex];

					float waveYPosition = this.gameConfig.WaveOffset * globalWaveNumber;

					GameObject.Instantiate(this.gameConfig.WavePrefab, waveYPosition * Vector3.up, Quaternion.identity);

					this.SpawnHazard(wavePattern.GroundLeft, 0, waveYPosition);
					this.SpawnHazard(wavePattern.GroundMiddle, 1, waveYPosition);
					this.SpawnHazard(wavePattern.GroundRight, 2, waveYPosition);

					++globalWaveNumber;
				}
			}
		}
	}

	private void SpawnHazard(Hazard.HazardType hazardType, int columnIndex, float waveYPosition)
	{
		if (hazardType == Hazard.HazardType.None)
		{
			return;
		}

		float hazardXPosition = (columnIndex - (this.gameConfig.ColumnCount / 2)) * this.gameConfig.MoveUnit;

		int colorIndex = Random.Range(0, this.playerCount);

		GameObject hazardGO = GameObject.Instantiate(this.gameConfig.HazardPrefab, hazardXPosition * Vector3.right + waveYPosition * Vector3.up, Quaternion.identity);
		Hazard hazard = hazardGO.GetComponent<Hazard>();
		hazard.Init(hazardType, colorIndex);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			// Toggle debug mode
			this.debugCamera.enabled = !this.debugCamera.enabled;
		}
		else if (Input.GetKeyDown(KeyCode.R))
		{
			// Restart
			SceneManager.LoadScene(0);
		}
	}
}
