using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum TouristeType
{
    Generic,
    Couple,
    Dog,
    Famille,
    JeuneZic,
    Surfeuse,
    Vendeur,
    Volley
}

public class touristSize : MonoBehaviour {
    public Vector2[] takenSize;

    public List<Vector2> ReservedTileIndex;
    public Vector2 tileOffset;

    public int BreakDownValue;

    public string GameOverSceneName = "GameOverScene";

    public int MinTurn = 2;
    public int MaxTurn = 6;

    int currentTurnLife = 0;
    bool destroy = false;

    public TouristeType m_Type = TouristeType.Generic;
    [SerializeField] UnityEvent SpawnEvent;
    [SerializeField] UnityEvent UnspawnEvent;
    public Vector2 GetTileOffset() {
            //calcul rectangle size
            int MaxX = -1;
            int MaxY = -1;
            for (int i = 0; i < takenSize.Length; i++)
            {
                if (takenSize[i].x > MaxX)
                {
                    MaxX = (int)takenSize[i].x;
                }
                if (takenSize[i].y > MaxY)
                {
                    MaxY = (int)takenSize[i].y;
                }
            }

            //set the offset ( in tile value )
            if (MaxX > 0)
            {
                tileOffset.x = (float)MaxX / 2f;
            }
            if (MaxY > 0)
            {
                tileOffset.y = (float)MaxY / 2f;
            }

            if(MaxX == 0 && MaxY == 0)
            {
                tileOffset.x = 0;
                tileOffset.y = 0;
        }
            return tileOffset;
    }
	
    public void SetOrderInLayer(int x, int y)
    {
        SpriteRenderer[] sprites= gameObject.GetComponentsInChildren<SpriteRenderer>();

        int orderInlayerValue = y * 100 + x;

        for(int i =0; i< sprites.Length; i++)
        {
            sprites[i].sortingOrder = -orderInlayerValue;
        }
    }

	void Start () {
        if (gameObject.GetComponent<PlayerComponent>() == null)
        {
            currentTurnLife = Random.Range(MinTurn, MaxTurn);

            //GameTurnManager.onChangeTurnEvent += handleChangeTurnEvent;
        }
    }

    public void handleChangeTurnEvent(TurnState state)
    {
        if (state == TurnState.GenerationTurn) //player turn is Over
        {
            currentTurnLife--;
            if(currentTurnLife == 1)
            {
                UnspawnEvent.Invoke();
            }
            if(currentTurnLife == 0)
            {
                Unspawn();
                //TouristSpawnManager.m_instance.m_SpawnedPrefab.Remove(gameObject);
                //GameTurnManager.onChangeTurnEvent -= handleChangeTurnEvent;
                Destroy(gameObject);
                destroy = true;
            }
        }
        else if (state == TurnState.PlayerTurn)
        {
        }
    }

    public void Unspawn()
    {
        TileGenerator.ReleaseTile(ReservedTileIndex);
    }

    public void SetReservedTileIndex(List<Vector2> inIndex)
    {
        TileGenerator.BusyTiles(inIndex, gameObject.GetComponent<PlayerComponent>() != null);
        ReservedTileIndex = inIndex;
    }
}
