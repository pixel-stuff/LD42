using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTurnManager : MonoBehaviour {

    #region Singleton
    public static GameTurnManager m_instance;
    void Awake()
    {
        if (m_instance == null)
        {
            //If I am the first instance, make me the Singleton
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != m_instance)
                Destroy(this.gameObject);
        }
    }
    #endregion Singleton

    public float PlayerTurnDuration = 3f;

    public float GenerationTurnDuration = 1f;

    public bool IsPlayerTurn = true;

    private float CurrentTimer = 0f;


    // Use this for initialization
    void Start () {
        CurrentTimer = PlayerTurnDuration;
    }
	
	// Update is called once per frame
	void Update () {
        CurrentTimer -= Time.deltaTime;

        if (CurrentTimer <= 0.0f)
        {
            if(IsPlayerTurn)
            {
                IsPlayerTurn = false;
                CurrentTimer = GenerationTurnDuration;
            }
            else
            {
                IsPlayerTurn = true;
                CurrentTimer = PlayerTurnDuration;
                TouristSpawnManager.m_instance.Generate = true;
            }
        }
    }
}
