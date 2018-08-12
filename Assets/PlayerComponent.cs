using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class MyIntEvent : UnityEvent<int>
{
}

[System.Serializable]
public class TouristModifier
{
    public TouristeType touristeType = TouristeType.Generic;
    public float modifier= 1.0f;
}

public class PlayerComponent : MonoBehaviour {

    public int CurrentTanValue = 0;
    public int MaxTanValue = 100;

    public int TanByTurn = 10;

    public int CurrentBreakDownValue = 100;


    public string GameOverSceneName = "GameOverScene";
    bool playerMove = false;

    [SerializeField] MyIntEvent TanApply;
    [SerializeField] MyIntEvent BreakDownApply;

    public Text TanText;
    public Text BreakText;


    public List<TouristModifier> m_modifier;


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
        else
        {
            GameTurnManager.m_instance.SkipPlayerTurn();
        }
    }
	
    public void handleChangeTurnEvent(TurnState state)
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
        CurrentTanValue += TanByTurn;
        TanApply.Invoke(CurrentTanValue);
        TanText.text = "TAN : " + CurrentTanValue;
        if (CurrentTanValue >= MaxTanValue)
        {
            //gameOver
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
        float modifier = 1.0f;
        foreach(TouristModifier touristeModifier in m_modifier)
        {
            if(touristeModifier.touristeType == touristSize.m_Type)
            {
                modifier = touristeModifier.modifier;
            }
        }

        if (touristSize.BreakDownValue > 0)
        {
            CurrentBreakDownValue -= (int)((float)touristSize.BreakDownValue * modifier);
            BreakDownApply.Invoke(CurrentBreakDownValue);
            BreakText.text = "Break : " + CurrentBreakDownValue;
            if (CurrentBreakDownValue <= 0)
            {
                //gameOver
                GameStateManager.setGameState(GameState.GameOver);
                SceneManager.LoadSceneAsync(touristSize.GameOverSceneName);
            }
        }
    }
}
