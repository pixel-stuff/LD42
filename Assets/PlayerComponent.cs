using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour {

    public int CurrentTanValue = 0;
    public int MaxTanValue = 100;

    public int CurrentBreakDownValue = 100;

    public void GoAtPosition(Vector3 position, int rotation)
    {
        if (position != transform.position || transform.localEulerAngles.z != rotation) //if player move
        {
            gameObject.SetActive(true);
            transform.position = position;
            Debug.Log("Rotation " + rotation);
            transform.localEulerAngles = new Vector3(0, 0, rotation);
            //set move to gamemanager
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
