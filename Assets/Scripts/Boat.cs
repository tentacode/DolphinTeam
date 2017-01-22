using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
	void Start ()
    {
        transform.localPosition = new Vector3(0, -Camera.main.orthographicSize + 60, 0);
	}
}
