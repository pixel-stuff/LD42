using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnspawnTourist : MonoBehaviour {

    public float duration = 1.0f;
    bool IsSpawn = false;
    private float startTime;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float t = (Time.time - startTime) / duration;
        if (t < 1.0f)
        {
            if (IsSpawn)
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0, 1, t));
            }
            else
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(1, 0, t));
            }
        }
    }

    public void Spawn()
    {
        IsSpawn = true;
        startTime = Time.time;
    }

    public void Unspawn()
    {
        IsSpawn = false;
        startTime = Time.time;
    }
}
