using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TileGenerator : MonoBehaviour {

    public GameObject m_prefabTile;
    public static int m_xSize = 10;
    public static int m_ySize = 10;

    public static List<TileComponent> s_TileComponent = new List<TileComponent>();

	// Use this for initialization
	void Start () {
        Sprite sprite = m_prefabTile.GetComponent<SpriteRenderer>().sprite;
        Vector2 SpriteSize = sprite.bounds.size;
        for(int x = 0; x< m_xSize; x++)
        {
            for (int y = 0; y < m_ySize; y++)
            {
                //set x y value
                GameObject newTile = (GameObject) Instantiate(m_prefabTile, this.transform.position + new Vector3(x * SpriteSize.x, y * SpriteSize.x, 0), Quaternion.identity);
                newTile.transform.SetParent(this.transform);
                newTile.GetComponent<TileComponent>().m_tileIndex = new Vector2(x, y);
                s_TileComponent.Add(newTile.GetComponent<TileComponent>());
                //newTile.transform.position = this.transform.position + Vector3(x * SpriteSize.x, y * SpriteSize.x, 0);
            }
        }
        this.transform.position -= new Vector3(m_xSize * SpriteSize.x/2f, m_ySize * SpriteSize.x/2f, 0);
    }

    public static List<Vector2> GetFreeTile()
    {
        List<Vector2> returnList = new List<Vector2>();
        foreach (TileComponent currentTile in s_TileComponent)
        {
            if (!currentTile.IsBusy)
            {
                returnList.Add(currentTile.m_tileIndex);
            }
            else
            {
                Debug.Log("reserved tile : " + currentTile.m_tileIndex);
            }
        }
        return returnList;
    }

    public static List<TileComponent> GetFreeTileComponent()
    {
        List<TileComponent> returnList = new List<TileComponent>();
        foreach (TileComponent currentTile in s_TileComponent)
        {
            if (!currentTile.IsBusy)
            {
                returnList.Add(currentTile);
            }
        }
        return returnList;
    }

    public static List<Vector2> GetFreeTileForPlayer()
    {
        List<Vector2> returnList = new List<Vector2>();
        foreach (TileComponent currentTile in s_TileComponent)
        {
            if (!currentTile.IsBusy || currentTile.IsBusyByPlayer)
            {
                returnList.Add(currentTile.m_tileIndex);
            }
        }
        return returnList;
    }

    public static void ReleaseTile(List<Vector2> indexArray)
    {
        foreach (TileComponent currentTile in s_TileComponent)
        {
            if(indexArray.Contains(currentTile.m_tileIndex))
            {
                currentTile.FreeTile();
            }
        }
    }

    public static void BusyTiles(List<Vector2> indexArray, bool IsPLayer)
    {
        foreach (TileComponent currentTile in s_TileComponent)
        {
            if (indexArray.Contains(currentTile.m_tileIndex))
            {
                currentTile.IsBusy = true;
                currentTile.IsBusyByPlayer = IsPLayer;
            }
        }
    }

    public static List<TileComponent> GetFreeTileNearPlayer(int min, int max)
    {
        List<Vector2> indexList = new List<Vector2>();
        foreach (TileComponent currentTile in GetTilePlayer())
        {
            for(int maxIndexX = -max; maxIndexX < max +1; maxIndexX++)
            {
                for (int maxIndexY = -max; maxIndexY < max + 1; maxIndexY++)
                {
                    indexList.Add(currentTile.m_tileIndex + new Vector2(maxIndexX, maxIndexY));
                }
            }
        }
        return GetFreeTileComponent(indexList);
    }

    public static List<TileComponent> GetTilePlayer()
    {
        List<TileComponent> returnList = new List<TileComponent>();
        foreach (TileComponent currentTile in s_TileComponent)
        {
            if(currentTile.IsBusyByPlayer)
            {
                returnList.Add(currentTile);
            }
        }

        return returnList;
    }

    public static List<TileComponent> GetFreeTileComponent(List<Vector2> index)
    {
        List<TileComponent> returnList = new List<TileComponent>();
        foreach (TileComponent currentTile in s_TileComponent)
        {
            if (index.Contains(currentTile.m_tileIndex) && !currentTile.IsBusy)
            {
                returnList.Add(currentTile);
            }
        }

        return returnList;
    }
}
