using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class TileComponent : MonoBehaviour {

    public GameObject playerSpawnPossibility;

    public Vector2 m_tileIndex;
    [SerializeField] UnityEvent IsOver;
    [SerializeField] UnityEvent IsOverExit;
    [SerializeField] UnityEvent IsSpawnPossible;
    //[SerializeField] UnityEvent IsSelected;
    bool m_IsOver = false;
    bool m_IsSpawnPossible = false;

    int m_rotation = -1;

    GameObject playerSpawnPossibilityGO;

    public bool IsBusy = false;
    public bool IsBusyByPlayer = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0) && m_IsSpawnPossible && !IsBusyByPlayer)
        {
            Debug.Log("SPAWN");
            if (PlayerManager.m_instance.m_player.isActiveAndEnabled)
            {
                PlayerManager.m_instance.m_player.GetComponent<touristSize>().Unspawn();
            }
            SpawnPlayer();
        }

        if(m_IsOver)
        {
            m_rotation = TileMatching(TileGenerator.GetFreeTileForPlayer(), playerSpawnPossibility.GetComponent<touristSize>().takenSize,1);
            if (m_rotation != -1 && !m_IsSpawnPossible)
            {
                DisplaySpawnPossibility();
                IsSpawnPossible.Invoke();
                m_IsSpawnPossible = true;
            }
        }
    }

    void OnMouseOver()
    {
        m_IsOver = true;
        IsOver.Invoke();
    }

    void OnMouseExit()
    {
        m_IsOver = false;
        IsOverExit.Invoke();
        m_IsSpawnPossible = false;
        UnDisplaySpawnPossibility();
    }

    void DisplaySpawnPossibility()
    {
        Sprite sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        Vector2 SpriteSize = sprite.bounds.size;
        Vector3 position = this.transform.position + new Vector3(SpriteSize.x * playerSpawnPossibility.GetComponent<touristSize>().GetTileOffset().x, SpriteSize.y * playerSpawnPossibility.GetComponent<touristSize>().GetTileOffset().y, 0);

        if (m_rotation == 1)
        {
            position = this.transform.position + new Vector3(SpriteSize.y * playerSpawnPossibility.GetComponent<touristSize>().GetTileOffset().y, SpriteSize.x * playerSpawnPossibility.GetComponent<touristSize>().GetTileOffset().x, 0);
        }
        playerSpawnPossibilityGO = (GameObject)Instantiate(playerSpawnPossibility, position, Quaternion.identity);

        if(m_rotation == 1)
        {
            playerSpawnPossibilityGO.transform.Rotate(0, 0, 90);
        }
    }

    void UnDisplaySpawnPossibility()
    {
        Destroy(playerSpawnPossibilityGO);
    }

    void SpawnPlayer()
    {
        Sprite sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        Vector2 SpriteSize = sprite.bounds.size;
        Vector3 position = this.transform.position + new Vector3(SpriteSize.x * PlayerManager.m_instance.m_player.GetComponent<touristSize>().GetTileOffset().x, SpriteSize.y * PlayerManager.m_instance.m_player.GetComponent<touristSize>().GetTileOffset().y, 0);

        if (m_rotation == 1)
        {
            position = this.transform.position + new Vector3(SpriteSize.y * PlayerManager.m_instance.m_player.GetComponent<touristSize>().GetTileOffset().y, SpriteSize.x * PlayerManager.m_instance.m_player.GetComponent<touristSize>().GetTileOffset().x, 0);
        }
        PlayerManager.m_instance.m_player.GoAtPosition(position, (m_rotation == 1) ? 90 : 0);
        FillTouristTileComponent(PlayerManager.m_instance.m_player.GetComponent<touristSize>());

    }

    public bool SpawnIfPossible(GameObject touristPrefab)
    {
        touristSize tourist = touristPrefab.GetComponent<touristSize>();
        int rotation = TileMatching(TileGenerator.GetFreeTile(), tourist.takenSize,0);
        if(rotation == -1)
        {
            return false;
        }
        Debug.Log(m_tileIndex);
        FillTouristTileComponent(tourist);
        Sprite sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        Vector2 SpriteSize = sprite.bounds.size;
        Vector3 position = this.transform.position + new Vector3(SpriteSize.x * touristPrefab.GetComponent<touristSize>().GetTileOffset().x, SpriteSize.y * touristPrefab.GetComponent<touristSize>().GetTileOffset().y, 0);

        if (m_rotation == 1)
        {
            position = this.transform.position + new Vector3(SpriteSize.y * touristPrefab.GetComponent<touristSize>().GetTileOffset().y, SpriteSize.x * touristPrefab.GetComponent<touristSize>().GetTileOffset().x, 0);
        }

        GameObject touristSpawnGO = (GameObject)Instantiate(touristPrefab, position, Quaternion.identity);
        touristSpawnGO.transform.localEulerAngles = new Vector3(0, 0, (m_rotation == 1) ? 90 : 0);
        return true;
    }

    void FillTouristTileComponent(touristSize tourist)
    {
        List<Vector2> returnList = new List<Vector2>();
        //playerSpawnPossibility.GetComponent<touristSize>().takenSize;
        Vector2[] touristSizesCopy = new Vector2[tourist.takenSize.Length];
        for (int i = 0; i < touristSizesCopy.Length; i++)
        {
            touristSizesCopy[i] = new Vector2(tourist.takenSize[i].x, tourist.takenSize[i].y);
        }

        foreach (Vector2 currentTouristSize in touristSizesCopy)
        {
            returnList.Add(m_tileIndex + currentTouristSize);
            Debug.Log("Reserved tile " + (m_tileIndex + currentTouristSize));
        }

        tourist.SetReservedTileIndex(returnList);
    }

    public void FreeTile()
    {
        IsBusy = false;
        IsBusyByPlayer = false;
    }

    int TileMatching(List<Vector2> selectedTile, Vector2[] touristSizes, int possibleRotation)
    {
        int rotation = 0;
        Vector2[] touristSizesCopy = new Vector2[touristSizes.Length];
        for (int i = 0; i < touristSizesCopy.Length; i++)
        {
            touristSizesCopy[i] = new Vector2(touristSizes[i].x, touristSizes[i].y);
        }
            //touristSizes.Copy(touristSizesCopy);
            do
        {
           // foreach (Vector2 startTile in selectedTile)
            //{
                bool isValid = true;
                foreach (Vector2 currentTouristSize in touristSizesCopy)
                {
                    Vector2 possibleTile = m_tileIndex + currentTouristSize;
                Debug.Log("Test " + possibleTile);
                    if (selectedTile.Contains(possibleTile))
                    {
                    Debug.Log("Tile OK for" + possibleTile);
                    //found
                }
                    else
                    {
                        isValid = false;
                        break;
                    }
                }
                if (isValid)
                {
                    return rotation;
                }
            //}
            //we rotate the tourist
            rotation++;
            for (int i = 0; i < touristSizesCopy.Length; i++)
            {
                //int yCopy = touristSizes[i].y;
                touristSizesCopy[i] = new Vector2(touristSizesCopy[i].y, -touristSizesCopy[i].x);
            }

        } while (rotation < possibleRotation +1);

        return -1;
    }
}
