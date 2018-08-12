using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TouristSpawningRule
{
    public TouristeType touristeType = TouristeType.Generic;
    public int MaxOccurence = 10;
}

public class TouristSpawnManager : MonoBehaviour {

    #region Singleton
    public static TouristSpawnManager m_instance;
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


    public List<GameObject> m_TouristPrefab;

    public List<TouristSpawningRule> m_TouristSpawningRules;

    private int minimumNearPlayer = 1;
    public int maxNearPlayer = 3;

    public bool GenerateNextFrame = false;

    public List<GameObject> m_SpawnedPrefab;

    // Use this for initialization
    void Start () {
        GameStateManager.onChangeStateEvent += handleGameStateChanged;
    }

    public void handleChangeTurnEvent(TurnState state)
    {
        

        if (state == TurnState.GenerationTurn) //player turn is Over
        {
            Generate();
        }
        else if (state == TurnState.PlayerTurn)
        {
        }

        List<int> nullIndexes = new List<int>();
        for (int i = 0; i < m_SpawnedPrefab.Count; i++)
        {
            if (m_SpawnedPrefab[i] == null)
            {
                nullIndexes.Add(i);
            }
            else
            {
                m_SpawnedPrefab[i].GetComponent<touristSize>().handleChangeTurnEvent(state);
            }
        }
        for (int j = 0; j < nullIndexes.Count; j++)
        {
            m_SpawnedPrefab.RemoveAt(nullIndexes[j]);
        }
    }

    void handleGameStateChanged(GameState newState)
    {
        if (newState == GameState.Playing)
        {

        }
        else
        {
           foreach(GameObject currentTouristGO in m_SpawnedPrefab)
            {
                if (currentTouristGO != null)
                {
                    currentTouristGO.GetComponent<touristSize>().Unspawn();
                    Destroy(currentTouristGO);
                }
            }
            m_SpawnedPrefab.Clear();
        }
    }

        // Update is called once per frame
        void Update () {
        if(GenerateNextFrame)
        {
            GenerateNextFrame = false;
            Generate();
        }
	}

    void Generate()
    {
        Debug.Log("Start generation");
        Shuffle(m_TouristPrefab);
        foreach (GameObject currentPrefab in m_TouristPrefab)
        {
            if (currentPrefab != null)
            {
                bool CanGenerate = true;
                touristSize currentSize = currentPrefab.GetComponent<touristSize>();
                
                int currentNumberOfThisType = 0;
                //check if we can selectThisOne
                for (int i = 0; i < m_SpawnedPrefab.Count; i++)
                {
                    if (m_SpawnedPrefab[i] != null && m_SpawnedPrefab[i].GetComponent<touristSize>().destroy)
                    {
                        Destroy(m_SpawnedPrefab[i]);
                        continue;
                    }
                    if (m_SpawnedPrefab[i] != null && m_SpawnedPrefab[i].GetComponent<touristSize>().m_Type == currentSize.m_Type)
                    {
                        currentNumberOfThisType++;
                    }
                }

                foreach(TouristSpawningRule rule in m_TouristSpawningRules)
                {
                    if(rule.touristeType == currentSize.m_Type)
                    {
                        if(currentNumberOfThisType >= rule.MaxOccurence)
                        {
                            CanGenerate = false;
                        }
                        break;
                    }
                }
                //

                if (CanGenerate)
                {
                    List<TileComponent> tileArray = TileGenerator.GetFreeTileNearPlayer(minimumNearPlayer, maxNearPlayer);
                    Debug.Log("TileArryNear : " + tileArray.Count);
                    Shuffle(tileArray);
                    foreach (TileComponent currentTileComponent in tileArray)
                    {
                        GameObject generatedGameObject = currentTileComponent.SpawnIfPossible(currentPrefab);
                        if (generatedGameObject != null)
                        {
                            m_SpawnedPrefab.Add(generatedGameObject);
                            return;
                        }
                    }

                    List<TileComponent> allTileArray = TileGenerator.GetFreeTileComponent();
                    Shuffle(allTileArray);

                    foreach (TileComponent currentTileComponent in allTileArray)
                    {
                        GameObject generatedGameObject = currentTileComponent.SpawnIfPossible(currentPrefab);
                        if (generatedGameObject != null)
                        {
                            m_SpawnedPrefab.Add(generatedGameObject);
                            return;
                        }
                    }
                }
            }
        }
    }

    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
