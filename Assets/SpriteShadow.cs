using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteShadow : MonoBehaviour {

	public Vector2 offset = new Vector2(-3, -3);

	private SpriteRenderer sprRndCaster;
	private SpriteRenderer sprRndShadow;

	private Transform transCaster;
	private Transform transShadow;

	// Use this for initialization
	void Start () {
        
		transCaster = transform;
		transShadow = new GameObject().transform;
		transShadow.parent = transCaster;
		transShadow.gameObject.name = "shadow";
        transShadow.localRotation = Quaternion.identity;

        sprRndCaster = GetComponent<SpriteRenderer>();
        sprRndShadow = transShadow.gameObject.AddComponent<SpriteRenderer>();
        sprRndShadow.sortingLayerName = sprRndCaster.sortingLayerName;
        sprRndShadow.sortingOrder = sprRndCaster.sortingOrder - 1;
	}
	
	// Update is called once per frame
	void Update () {
        transShadow.position = new Vector2(transCaster.position.x + offset.x,
                                           transCaster.position.y + offset.y);

        sprRndShadow.sprite = sprRndCaster.sprite;
    }
}
