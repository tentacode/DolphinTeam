using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WaveLane : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float offset;
    [SerializeField]
    private GameConfig gameConfig;
    private float initialY;

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        initialY = this.transform.position.y;
    }

	void Update ()
    {
        this.offset += Time.deltaTime * this.gameConfig.ScrollSpeed;
        if ( this.offset >= 200 )
        {
            this.offset -= 200;
        }
        Vector3 pos = this.transform.position;
        pos.y = initialY-this.offset;
        this.transform.position = pos;
	}
}
