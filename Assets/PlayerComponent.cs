using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour {


    public void GoAtPosition(Vector3 position, int rotation)
    {
        gameObject.SetActive(true);
        transform.position = position;
        Debug.Log("Rotation " + rotation);
        transform.localEulerAngles = new Vector3(0, 0, rotation);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
