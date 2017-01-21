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

	private List<PatternGroup> difficultyPatternGroups = new List<PatternGroup>();

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

		// Generate pattern groups
		int patternGroupCount = Random.Range(this.gameConfig.MinPatternGroupCount, this.gameConfig.MaxPatternGroupCount + 1);
		for (int patternGroupNumber = 1; patternGroupNumber <= patternGroupCount; ++patternGroupNumber)
		{
			// Compute pattern group difficulty
			float difficultyTime = (float)patternGroupNumber / (float)patternGroupCount;
			float minDifficulty = this.gameConfig.MinDifficultyCurve.Evaluate(difficultyTime);
			float maxDifficulty = this.gameConfig.MaxDifficultyCurve.Evaluate(difficultyTime);

			// Filter pattern groups for difficulty
			this.difficultyPatternGroups.Clear();
			for (int patternGroupIndex = 0; patternGroupIndex < this.gameConfig.PatternGroups.Length; ++patternGroupIndex)
			{
				PatternGroup patternGroup = this.gameConfig.PatternGroups[patternGroupIndex];
				if (patternGroup.Difficulty >= minDifficulty && patternGroup.Difficulty <= maxDifficulty)
				{
					this.difficultyPatternGroups.Add(patternGroup);
				}
			}

			if (this.difficultyPatternGroups.Count < 1)
			{
				Debug.LogErrorFormat("No pattern group for difficulty between {0} and {1}!", minDifficulty, maxDifficulty);
				break;
			}

			// Generate wave group
			PatternGroup difficultyPatternGroup = this.difficultyPatternGroups[Random.Range(0, this.difficultyPatternGroups.Count)];
			for (int patternIndex = 0; patternIndex < difficultyPatternGroup.Patterns.Length; ++patternIndex)
			{
				// Generate wave
				Pattern pattern = difficultyPatternGroup.Patterns[patternIndex];

				float waveYPosition = this.gameConfig.WaveOffset * globalWaveNumber;

				this.SpawnHazard(pattern.GroundLeft, 0, waveYPosition);
				this.SpawnHazard(pattern.GroundMiddle, 1, waveYPosition);
				this.SpawnHazard(pattern.GroundRight, 2, waveYPosition);

				++globalWaveNumber;
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
