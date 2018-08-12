using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnState
{
    PlayerTurn,
    GenerationTurn
}

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


    public static TurnState m_TurnState = TurnState.PlayerTurn; 
    public static Action<TurnState> onChangeTurnEvent;


    public float PlayerTurnDuration = 3f;

    public float GenerationTurnDuration = 1f;

    //public bool IsPlayerTurn = true;
    public bool AuthoriseGeneration = true;

    private float CurrentTimer = 0f;


    // Use this for initialization
    void Start () {
        GameStateManager.onChangeStateEvent += handleGameStateChanged;
        CurrentTimer = PlayerTurnDuration;
    }
	
    void handleGameStateChanged(GameState newState)
    {
        if(newState == GameState.Playing)
        {
            AuthoriseGeneration = true;
            CurrentTimer = PlayerTurnDuration;
            SetTurn(TurnState.PlayerTurn);
        }
        else
        {
            AuthoriseGeneration = false;
        }
    }
    // Update is called once per frame
    void Update () {
        if (AuthoriseGeneration)
        {
            CurrentTimer -= Time.deltaTime;

            if (CurrentTimer <= 0.0f)
            {
                if (IsPlayerTurn())
                {
                    SetTurn(TurnState.GenerationTurn);
                    CurrentTimer = GenerationTurnDuration;
                }
                else
                {
                    SetTurn(TurnState.PlayerTurn);
                    CurrentTimer = PlayerTurnDuration;
                }
            }
        }
    }

    public bool IsPlayerTurn()
    {
        return m_TurnState == TurnState.PlayerTurn;
    }

    public void SetTurn(TurnState state)
    {
        if (state != m_TurnState)
        {
            Debug.Log("TurnChanged: " + state);
            m_TurnState = state;
            if (onChangeTurnEvent != null)
            {
                onChangeTurnEvent(state);
            }
        }
    }
}
