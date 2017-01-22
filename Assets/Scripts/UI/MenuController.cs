using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public List<GameObject> panels;
	public GameObject initialPanel;
	public List<Image> masterSeedSymbols;
	public List<Image> slaveSeedSymbols;
	public Image playerSymbol;
	public List<Sprite> playerSymbolSprites;
	public List<Sprite> playerCountSprites;

	private int playerNumber;
	private int randomSeed;
	private int playerCount = 2;

	void Start()
	{
		SwitchToLanding();
	}

	void HideEverythingBut(string panelName)
	{
        foreach(GameObject go in panels) 
        {
			go.SetActive(go.name == panelName);
		}
	}

	public void ChoosePlayerNumber(int number)
	{
		playerNumber = number;
		masterSeedSymbols[3].sprite = playerSymbolSprites[playerNumber - 2];

		Game.Instance.SetLocalPlayerIndex(0);
		Game.Instance.SetPlayerCount(playerNumber);

		randomSeed = Random.Range(0, 64);
		Game.Instance.GenerateLevel(randomSeed);

		string base4seed = IntToBase4(randomSeed).PadLeft(3, '0');
		masterSeedSymbols[0].sprite = playerSymbolSprites[(int)base4seed[0] - '0'];
		masterSeedSymbols[1].sprite = playerSymbolSprites[(int)base4seed[1] - '0'];
		masterSeedSymbols[2].sprite = playerSymbolSprites[(int)base4seed[2] - '0'];

		SwitchToSeedDisplay();
	}

	public static string IntToBase4(int value)
    {
		char[] baseChars = new char[] { '0','1','2','3'};
        string result = string.Empty;
        int targetBase = baseChars.Length;

        do
        {
            result = baseChars[value % targetBase] + result;
            value = value / targetBase;
        } 
        while (value > 0);

        return result;
    }

	public void SwitchToLanding()
	{
		HideEverythingBut("Landing");
	}

	public void SwitchToModeChoice()
	{
		HideEverythingBut("ModeChoice");
	}

	public void SwitchToPlayerNumber()
	{
		HideEverythingBut("PlayerNumber");
	}

	public void SwitchToSeedInput()
	{
		HideEverythingBut("SeedInput");
	}

	public void SwitchToSeedDisplay()
	{
		HideEverythingBut("SeedDisplay");
	}

	public void SwitchToReady()
	{
		HideEverythingBut("Ready");
	}

	public void StartGame()
	{
		HideEverythingBut("🤔");
	}

	public void SymbolInputUp(int symbolIndex)
	{
		Image inputSymbol = slaveSeedSymbols[symbolIndex];
		Sprite sprite = inputSymbol.sprite;

		switch (sprite.name) {
			case "Symbols_0":
				inputSymbol.sprite = playerSymbolSprites[1];
				break;
			case "Symbols_1":
				inputSymbol.sprite = playerSymbolSprites[2];
				break;
			case "Symbols_2":
				inputSymbol.sprite = playerSymbolSprites[3];
				break;
			default:
				inputSymbol.sprite = playerSymbolSprites[0];
				break;
		}
	}

	public void SymbolInputDown(int symbolIndex)
	{
		Image inputSymbol = slaveSeedSymbols[symbolIndex];
		Sprite sprite = inputSymbol.sprite;

		switch (sprite.name) {
			case "Symbols_3":
				inputSymbol.sprite = playerSymbolSprites[2];
				break;
			case "Symbols_2":
				inputSymbol.sprite = playerSymbolSprites[1];
				break;
			case "Symbols_1":
				inputSymbol.sprite = playerSymbolSprites[0];
				break;
			default:
				inputSymbol.sprite = playerSymbolSprites[3];
				break;
		}
	}

	public void PlayerSymbolUp()
	{
		Sprite sprite = playerSymbol.sprite;

		switch (sprite.name) {
			case "PlayerCount_0":
				if (playerCount == 2) {
					return;
				}

				playerSymbol.sprite = playerCountSprites[1];
				break;
			case "PlayerCount_1":
				if (playerCount <= 3) {
					playerSymbol.sprite = playerCountSprites[0];
				}

				playerSymbol.sprite = playerCountSprites[2];
				break;
			case "PlayerCount_2":
				if (playerCount <= 4) {
					playerSymbol.sprite = playerCountSprites[0];
				}

				playerSymbol.sprite = playerCountSprites[3];
				break;
			case "PlayerCount_3":
				playerSymbol.sprite = playerCountSprites[0];
				break;
		}
	}

	public void PlayerSymbolDown()
	{
		Sprite sprite = playerSymbol.sprite;

		switch (sprite.name) {
			case "PlayerCount_0":
				switch (playerCount) {
					case 5:
						playerSymbol.sprite = playerCountSprites[3];
						break;
					case 4:
						playerSymbol.sprite = playerCountSprites[2];
						break;
					case 3:
						playerSymbol.sprite = playerCountSprites[1];
						break;
					default:
						return;
				}

				break;
			case "PlayerCount_1":
				playerSymbol.sprite = playerCountSprites[0];
				break;
			case "PlayerCount_2":
				playerSymbol.sprite = playerCountSprites[1];
				break;
			case "PlayerCount_3":
				playerSymbol.sprite = playerCountSprites[2];
				break;
		}
	}
}
