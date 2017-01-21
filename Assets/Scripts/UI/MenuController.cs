using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public List<GameObject> panels;
	public GameObject initialPanel;
	public List<Image> masterSeedSymbols;
	public List<Sprite> playerSymbolSprites;

	private int playerNumber;
	private int randomSeed;

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
}
