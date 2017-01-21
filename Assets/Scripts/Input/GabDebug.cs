using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GabDebug : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (DolphinInput.IsJumping()) {
			Debug.Log("Is Jumpin");
		}	
		if (DolphinInput.IsGoingLeft()) {
			Debug.Log("Is goin left");
		}	
		if (DolphinInput.IsGoingRight()) {
			Debug.Log("Is goin right");
		}	
	}
}
