using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerComponent : MonoBehaviour {

    public int CurrentTanValue = 0;
    public int MaxTanValue = 100;

    public int TanByTurn = 10;

    public int CurrentBreakDownValue = 100;


    public string GameOverSceneName = "GameOverScene";
    bool playerMove = false;

    [SerializeField] UnityEvent TanApply;
    [SerializeField] UnityEvent BreakDownApply;

    public void GoAtPosition(Vector3 position, int rotation)
    {
        if (position != transform.position || transform.localEulerAngles.z != rotation) //if player move
        {
            playerMove = true;
            gameObject.SetActive(true);
            transform.position = position;
            transform.localEulerAngles = new Vector3(0, 0, rotation);
            //set move to gamemanager
        }
    }
	// Use this for initialization
	void Start () {
        GameTurnManager.onChangeTurnEvent += handleChangeTurnEvent;

    }
	
    void handleChangeTurnEvent(TurnState state)
    {
        if(state == TurnState.GenerationTurn) //player turn is Over
        {
            if(!playerMove)
            {
                ApplyTan();
                ApplyNearTourist();
            }
        }
        else if (state == TurnState.PlayerTurn)
        {
            playerMove = false;
        }
    }

    void ApplyTan()
    {
        Debug.Log("ApplyTan:");
        CurrentTanValue += TanByTurn;
        TanApply.Invoke();
        if(CurrentTanValue >= MaxTanValue)
        {
            //gameOver
            GameTurnManager.onChangeTurnEvent -= handleChangeTurnEvent;
            GameStateManager.setGameState(GameState.GameOver);
            SceneManager.LoadSceneAsync(GameOverSceneName);
        }
    }

    void ApplyNearTourist()
    {
        List<Vector2> nearPlayerTourist = TileGenerator.GetTouristTileNearPlayer(1);

        foreach(GameObject currentTouriste in TouristSpawnManager.m_instance.m_SpawnedPrefab)
        {
            touristSize currentTouristSize = currentTouriste.GetComponent<touristSize>();
            List<Vector2> touristeTiles = currentTouristSize.ReservedTileIndex;
            foreach(Vector2 currentTileNearPlayerTourist in nearPlayerTourist)
            {
                if(touristeTiles.Contains(currentTileNearPlayerTourist))
                {
                    ApplyTouristDebuf(currentTouristSize);
                    break;
                }
            }
        }
    }

    void ApplyTouristDebuf(touristSize touristSize)
    {
        Debug.Log("TouristDebuf: " + touristSize.BreakDownValue);
        if (touristSize.BreakDownValue > 0)
        {
            CurrentBreakDownValue -= touristSize.BreakDownValue;
            BreakDownApply.Invoke();
        if (CurrentBreakDownValue <= 0)
            {
                //gameOver
                GameTurnManager.onChangeTurnEvent -= handleChangeTurnEvent;
                GameStateManager.setGameState(GameState.GameOver);
                SceneManager.LoadSceneAsync(touristSize.GameOverSceneName);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
