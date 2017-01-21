using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	public List<GameObject> panels;
	public GameObject initialPanel;

	public void Start()
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
}
