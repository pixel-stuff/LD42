using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirculaireJauge : MonoBehaviour {

    public List<Sprite> SortTab;
    public float animationTime;


    public int CurrentIndex = 0;
    public int targetIndex = 0;

    public int MaxValue = 100;

    private float startTime;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float t = (Time.time - startTime) / animationTime;
        spriteRenderer.sprite = SortTab[(int)(Mathf.SmoothStep(CurrentIndex, targetIndex, t))];
    }

    public void ChangeValue(int value)
    {
        CurrentIndex = targetIndex;
        startTime = Time.time;
        //spriteRenderer.SetColor(Color.white);
        float percent = ((float)value) / MaxValue;
        targetIndex = (int)(percent * SortTab.Count) - 1;
        if (targetIndex >= SortTab.Count)
        {
            targetIndex = SortTab.Count - 1;

        } else if (targetIndex < 0)
        {
            targetIndex = 0;
        }
    }
}
