using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SeletedTileManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    static Vector3 TileMatching(Vector2[] selectedTile, Vector2[] touristSizes)
    {
        int rotation =  0;
        do
        {
            foreach (Vector2 startTile in selectedTile)
            {
                bool isValid = true;
                foreach (Vector2 touristSize in touristSizes)
                {
                    Vector2 possibleTile = startTile + touristSize;
                    int pos = Array.IndexOf(selectedTile, possibleTile);
                    if (pos > -1)
                    {
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
                    return new Vector3(startTile.x, startTile.y, rotation);
                }
            }
            //we rotate the tourist
            rotation++;
            for ( int i = 0; i< touristSizes.Length;i++)
            {
                //int yCopy = touristSizes[i].y;
                touristSizes[i] = new Vector2(-touristSizes[i].y, touristSizes[i].x);
            }

        } while (rotation < 4);

        return new Vector3(-1, -1, -1);
    }
}
