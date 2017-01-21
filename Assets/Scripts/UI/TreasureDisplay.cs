using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureDisplay : MonoBehaviour
{
    [SerializeField]
    private Text text;

    public void UpdateDisplayCount(int treasureCount)
    {
        text.text = treasureCount.ToString();
    }
}
