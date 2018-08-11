using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touristSize : MonoBehaviour {
    public Vector2[] takenSize;

    private List<Vector2> ReservedTileIndex;
	// Use this for initialization
	void Start () {
		
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
