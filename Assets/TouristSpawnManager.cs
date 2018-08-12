using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private int minimumNearPlayer = 1;
    public int maxNearPlayer = 3;

    public bool Generate = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Generate)
        {
            Generate = false;
            Shuffle(m_TouristPrefab);
            foreach(GameObject currentPrefab in m_TouristPrefab)
            {
                touristSize currentSize = currentPrefab.GetComponent<touristSize>();
                List<TileComponent> tileArray = TileGenerator.GetFreeTileNearPlayer(minimumNearPlayer, maxNearPlayer);
                Debug.Log(tileArray.Count);
                Shuffle(tileArray);
                foreach (TileComponent currentTileComponent in tileArray)
                {
                    if(currentTileComponent.SpawnIfPossible(currentPrefab))
                    {
                        return;
                    }
                }

                List<TileComponent> allTileArray = TileGenerator.GetFreeTileComponent();
                Shuffle(allTileArray);

                foreach (TileComponent currentTileComponent in allTileArray)
                {
                    if (currentTileComponent.SpawnIfPossible(currentPrefab))
                    {
                        return;
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
