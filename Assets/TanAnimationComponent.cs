using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanAnimationComponent : MonoBehaviour {
    public Color OriginalColor = Color.white;
    public Color EndTanColor = Color.blue;
    private Color CurrentColor = Color.white;
    private Color TargetColor = Color.white;
    public float speed = 1.0f;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.color = Color.Lerp(CurrentColor, TargetColor,Time.time * speed);
    }

    public void handleTanChange(int tanvalue)
    {
        CurrentColor = TargetColor;
        TargetColor = Color.Lerp(OriginalColor, EndTanColor, ((float)tanvalue) / (float)PlayerManager.m_instance.m_player.GetComponent<PlayerComponent>().MaxTanValue);
    }
}
