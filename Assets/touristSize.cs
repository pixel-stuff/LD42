using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touristSize : MonoBehaviour {
    public Vector2[] takenSize;

    private List<Vector2> ReservedTileIndex;
    private Vector2 tileOffset;
    // Use this for initialization
    public Vector2 GetTileOffset() {
        if (tileOffset == Vector2.zero)
        {
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
        }
        return tileOffset;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Unspawn()
    {
        TileGenerator.ReleaseTile(ReservedTileIndex);
    }

    public void SetReservedTileIndex(List<Vector2> inIndex)
    {
        Debug.Log("SetReservedTileIndex");
        TileGenerator.BusyTiles(inIndex, gameObject.GetComponent<PlayerComponent>() != null);
        ReservedTileIndex = inIndex;
    }
}
