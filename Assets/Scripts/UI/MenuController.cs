﻿using System.Collections.Generic;
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
	public List<Sprite> playerIndexSprites;

	public GameObject upPlayerButton;
	public GameObject downPlayerButton;

	private int seed;
	private int playerIndex;
	private int playerCount;

	public GameObject gameUI;

	void Start()
	{
		gameUI.SetActive(false);
		SwitchToModeChoice();
		SetPlayerCount(2);
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
		playerCount = number;
		masterSeedSymbols[3].sprite = playerSymbolSprites[playerCount - 2];

		Game.Instance.SetLocalPlayerIndex(0);
		Game.Instance.SetPlayerCount(playerCount);

		seed = Random.Range(0, 64);
		Game.Instance.GenerateLevel(seed);

		string base4seed = IntToBase4(seed).PadLeft(3, '0');
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
		Application.LoadLevel(0);
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
		Game.Instance.StartGame();
		gameUI.SetActive(true);
	}

	public void SymbolInputUp(int symbolIndex)
	{
		Image inputSymbol = slaveSeedSymbols[symbolIndex];
		Sprite sprite = inputSymbol.sprite;

		switch (sprite.name) {
			case "Seed_starfish":
				inputSymbol.sprite = playerSymbolSprites[1];
				break;
			case "Seed_sushi":
				inputSymbol.sprite = playerSymbolSprites[2];
				break;
			case "Seed_fishbone":
				inputSymbol.sprite = playerSymbolSprites[3];
				break;
			default:
				inputSymbol.sprite = playerSymbolSprites[0];
				break;
		}

		// updating player count
		if (symbolIndex == 3) {
			switch (inputSymbol.sprite.name) {
				case "Seed_starfish":
					SetPlayerCount(2);
					break;
				case "Seed_sushi":
					SetPlayerCount(3);
					break;
				case "Seed_fishbone":
					SetPlayerCount(4);
					break;
				case "Seed_tunacan":
					SetPlayerCount(5);
					break;
			}
		}

		ComputeInputSeed();
	}

	public void SymbolInputDown(int symbolIndex)
	{
		Image inputSymbol = slaveSeedSymbols[symbolIndex];
		Sprite sprite = inputSymbol.sprite;

		switch (sprite.name) {
			case "Seed_tunacan":
				inputSymbol.sprite = playerSymbolSprites[2];
				break;
			case "Seed_fishbone":
				inputSymbol.sprite = playerSymbolSprites[1];
				break;
			case "Seed_sushi":
				inputSymbol.sprite = playerSymbolSprites[0];
				break;
			default:
				inputSymbol.sprite = playerSymbolSprites[3];
				break;
		}

		// updating player count
		if (symbolIndex == 3) {
			switch (inputSymbol.sprite.name) {
				case "Seed_starfish":
					SetPlayerCount(2);
					break;
				case "Seed_sushi":
					SetPlayerCount(3);
					break;
				case "Seed_fishbone":
					SetPlayerCount(4);
					break;
				case "Seed_tunacan":
					SetPlayerCount(5);
					break;
			}
		}

		ComputeInputSeed();
	}

	void ComputeInputSeed()
	{
		seed = 0;

		int spriteIndex0 = GetSpriteSymbolIndex(slaveSeedSymbols[0].sprite);
		seed += spriteIndex0 * 16;

		int spriteIndex1 = GetSpriteSymbolIndex(slaveSeedSymbols[1].sprite);
		seed += spriteIndex1 * 4;

		int spriteIndex2 = GetSpriteSymbolIndex(slaveSeedSymbols[2].sprite);
		seed += spriteIndex2 * 1;
	}

	int GetSpriteSymbolIndex(Sprite sprite)
	{
		switch (sprite.name)
		{
			case "Seed_starfish":
				return 0;
			case "Seed_sushi":
				return 1;
			case "Seed_fishbone":
				return 2;
		}

		return 3;
	}

	void SetPlayerCount(int count)
	{
		playerCount = count;
		
		upPlayerButton.SetActive(playerCount > 2);
		downPlayerButton.SetActive(playerCount > 2);

		if (playerCount == 2) {
			playerSymbol.sprite = playerIndexSprites[0];
		}

		switch (playerSymbol.sprite.name) {
			case "player_green":
				if (playerCount == 2) {
					playerSymbol.sprite = playerIndexSprites[0];
				}
				break;
			case "player_purple":
				if (playerCount <= 3) {
					playerSymbol.sprite = playerIndexSprites[1];
				}
				break;
			case "player_yellow":
				if (playerCount <= 4) {
					playerSymbol.sprite = playerIndexSprites[2];
				}
				break;
		}

		ComputePlayerIndex();
	}

	public void PlayerSymbolUp()
	{
		Sprite sprite = playerSymbol.sprite;

		switch (sprite.name) {
			case "player_blue":
				if (playerCount == 2) {
					return;
				}

				playerSymbol.sprite = playerIndexSprites[1];
				break;
			case "player_green":
				if (playerCount <= 3) {
					playerSymbol.sprite = playerIndexSprites[0];
					break;
				}

				playerSymbol.sprite = playerIndexSprites[2];
				break;
			case "player_purple":
				if (playerCount <= 4) {
					playerSymbol.sprite = playerIndexSprites[0];
					break;
				}

				playerSymbol.sprite = playerIndexSprites[3];
				break;
			case "player_yellow":
				playerSymbol.sprite = playerIndexSprites[0];
				break;
		}

		ComputePlayerIndex();
	}

	public void PlayerSymbolDown()
	{
		Sprite sprite = playerSymbol.sprite;

		switch (sprite.name) {
			case "player_blue":
				switch (playerCount) {
					case 5:
						playerSymbol.sprite = playerIndexSprites[3];
						break;
					case 4:
						playerSymbol.sprite = playerIndexSprites[2];
						break;
					case 3:
						playerSymbol.sprite = playerIndexSprites[1];
						break;
					default:
						return;
				}

				break;
			case "player_green":
				playerSymbol.sprite = playerIndexSprites[0];
				break;
			case "player_purple":
				playerSymbol.sprite = playerIndexSprites[1];
				break;
			case "player_yellow":
				playerSymbol.sprite = playerIndexSprites[2];
				break;
		}

		ComputePlayerIndex();
	}

	public void ComputePlayerIndex()
	{
		// host is player index 0, slave are players 1 to 4
		switch (playerSymbol.sprite.name) {
			case "player_blue":
				playerIndex = 1;
				break;
			case "player_green":
				playerIndex = 2;
				break;
			case "player_purple":
				playerIndex = 3;
				break;
			case "player_yellow":
				playerIndex = 4;
				break;
		}
	}

	public void PlaySlave()
	{
		Game.Instance.SetLocalPlayerIndex(playerIndex);
		Game.Instance.SetPlayerCount(playerCount);
		Game.Instance.GenerateLevel(seed);

		SwitchToReady();
	}
}
