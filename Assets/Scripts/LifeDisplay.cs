using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject lifeUnitPrefab;

    private List<GameObject> displayedLifeUnit;

    private void Awake()
    {
        displayedLifeUnit = new List<GameObject>();
    }

    public void UpdateDisplayedLifeCount(int newHeartCount)
    {
        int diffCount = newHeartCount - this.displayedLifeUnit.Count;
        if (diffCount > 0)
        {
            for (int i = 0; i < diffCount; ++i)
            {
                GameObject newDisplayedLifeUnit = GameObject.Instantiate(this.lifeUnitPrefab);
                RectTransform newTransform = newDisplayedLifeUnit.GetComponent<RectTransform>();
                newTransform.SetParent(this.transform);
                newTransform.anchoredPosition = new Vector2(this.displayedLifeUnit.Count * 30.0f, 0.0f);
                this.displayedLifeUnit.Add(newDisplayedLifeUnit);
            }
        }
        else if (diffCount < 0)
        {
            diffCount *= -1;
            for (int i = 0; i < diffCount; ++i)
            {
                int idx = this.displayedLifeUnit.Count - diffCount + i;
                GameObject.Destroy(this.displayedLifeUnit[idx]);
                this.displayedLifeUnit.RemoveAt(idx);
            }
        }
    }
}