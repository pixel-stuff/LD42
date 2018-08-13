using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmogiPatience : MonoBehaviour {

    public List<Sprite> emogiList;
    public int MaxValue = 70;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public void HandleChange(int value) {
        float percent = ((float)value) / MaxValue;
        int targetIndex = (int)(percent * emogiList.Count) - 1;

        if (targetIndex >= emogiList.Count)
        {
            targetIndex = emogiList.Count - 1;

        }
        else if (targetIndex < 0)
        {
            targetIndex = 0;
        }
        spriteRenderer.sprite = emogiList[targetIndex];
    }
}
