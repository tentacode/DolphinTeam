using System.Collections.Generic;
using UnityEngine;

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

	private List<int> freeColumns = new List<int>();

	public int LocalPlayerIndex { get { return this.localPlayerIndex; } }

	public Game()
	{
		Game.Instance = this;
	}

	private void Start()
	{
		//Random.InitState(this.seed);

		Hazard.HazardType[] hazardTypes = System.Enum.GetValues(typeof(Hazard.HazardType)) as Hazard.HazardType[];

		int stepCount = Random.Range(this.gameConfig.MinStepCount, this.gameConfig.MaxStepCount + 1);
		for (int stepIndex = 0; stepIndex < stepCount; ++stepIndex)
		{
			GameObject stepGO = GameObject.Instantiate(this.gameConfig.StepPrefab, this.gameConfig.StepOffset * stepIndex * Vector3.up, Quaternion.identity);
			Step step = stepGO.GetComponent<Step>();

			this.freeColumns = new List<int>();
			for (int columnIndex = 0; columnIndex < this.gameConfig.ColumnCount; ++columnIndex)
			{
				this.freeColumns.Add(columnIndex);
			}

			int hazardCount = Random.Range(this.gameConfig.MinHazardCount, this.gameConfig.MaxHazardCount + 1);
			Hazard.Config[] hazardConfigs = new Hazard.Config[this.gameConfig.ColumnCount];
			for (int hazardIndex = 0; hazardIndex < hazardCount; ++hazardIndex)
			{
				Hazard.Config hazardConfig = new Hazard.Config();

				int columnIndex = Random.Range(0, this.freeColumns.Count);
				this.freeColumns.RemoveAt(columnIndex);

				hazardConfig.ColorIndex = Random.Range(0, this.playerCount);
				hazardConfig.Type = hazardTypes[Random.Range(0, hazardTypes.Length)];

				hazardConfigs[columnIndex] = hazardConfig;
			}

			step.Init(hazardConfigs);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			this.debugCamera.enabled = !this.debugCamera.enabled;
		}
	}
}
