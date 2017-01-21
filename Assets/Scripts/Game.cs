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
		for (int sequenceIndex = 0; sequenceIndex < this.gameConfig.Sequences.Length; ++sequenceIndex)
		{
			Sequence sequence = this.gameConfig.Sequences[sequenceIndex];
			Debug.LogWarningFormat("Sequence > {0}", sequence.name);

			// Generate pattern groups
			for (int waveGroupPatternNumber = 1; waveGroupPatternNumber <= sequence.WaveGroupCount; ++waveGroupPatternNumber)
			{
				// Compute pattern group difficulty
				float difficultyTime = (float)waveGroupPatternNumber / (float)sequence.WaveGroupCount;
				float minDifficulty = sequence.MinDifficultyCurve.Evaluate(difficultyTime);
				float maxDifficulty = sequence.MaxDifficultyCurve.Evaluate(difficultyTime);
				Debug.LogFormat("minDifficulty > {0}", minDifficulty);
				Debug.LogFormat("maxDifficulty > {0}", maxDifficulty);

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
				Debug.LogFormat("difficultyWaveGroupPatterns.Count > {0}", this.difficultyWaveGroupPatterns.Count);

				if (this.difficultyWaveGroupPatterns.Count < 1)
				{
					Debug.LogErrorFormat("No pattern group for difficulty between {0} and {1}!", minDifficulty, maxDifficulty);
					break;
				}

				// Generate wave group
				WaveGroupPattern difficultyWaveGroupPattern = this.difficultyWaveGroupPatterns[Random.Range(0, this.difficultyWaveGroupPatterns.Count)];
				Debug.LogFormat("WaveGroup > {0}",  difficultyWaveGroupPattern.name);
				for (int patternIndex = 0; patternIndex < difficultyWaveGroupPattern.WavePatterns.Length; ++patternIndex)
				{
					// Generate wave
					WavePattern wavePattern = difficultyWaveGroupPattern.WavePatterns[patternIndex];
					Debug.LogFormat("Wave > {0}", wavePattern.name);

					float waveYPosition = this.gameConfig.WaveOffset * globalWaveNumber;

					GameObject waveGO = GameObject.Instantiate(this.gameConfig.WavePrefab, waveYPosition * Vector3.up + this.gameConfig.WavePrefab.transform.position.z * Vector3.forward, Quaternion.identity);
					waveGO.name = wavePattern.name;

					this.SpawnHazard(wavePattern.GroundLeft, 0, waveYPosition, wavePattern.NoHazardHiding);
					this.SpawnHazard(wavePattern.GroundMiddle, 1, waveYPosition, wavePattern.NoHazardHiding);
					this.SpawnHazard(wavePattern.GroundRight, 2, waveYPosition, wavePattern.NoHazardHiding);

					++globalWaveNumber;
				}
			}
		}
	}

	private void SpawnHazard(Hazard.HazardType hazardType, int columnIndex, float waveYPosition, bool noHazardHiding)
	{
		if (hazardType == Hazard.HazardType.None)
		{
			return;
		}

		Debug.LogFormat("Hazard > {0}", hazardType);

		float hazardXPosition = (columnIndex - (this.gameConfig.ColumnCount / 2)) * this.gameConfig.MoveUnit;
		int colorIndex = Random.Range(0, this.playerCount);

		GameObject hazardGO = GameObject.Instantiate(this.gameConfig.HazardPrefab, hazardXPosition * Vector3.right + waveYPosition * Vector3.up + this.gameConfig.HazardPrefab.transform.position.z * Vector3.forward, Quaternion.identity);
		hazardGO.name = hazardType.ToString();

		Hazard hazard = hazardGO.GetComponent<Hazard>();
		hazard.Init(hazardType, colorIndex, noHazardHiding);
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
