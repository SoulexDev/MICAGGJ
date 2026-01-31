using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomTileset", menuName = "My Assets/Room Tileset")]
public class RoomTileset : ScriptableObject
{
    public TilePiece startingTile;
    public List<TilesetEntry> availableTiles;

    private List<TilePiece> tileList;


    public void BuildTileList()
    {
        tileList = new List<TilePiece>();
        for (int i = 0; i < availableTiles.Count; i++)
        {
            for (int j = 0; j < availableTiles[i].count; j++)
            {
                tileList.Add(availableTiles[i].tile);
            }
        }
    }

    public TilePiece TakePiece()
    {
        if(tileList.Count == 0)
        {
            return null;
        }

        int tileInd = UnityEngine.Random.Range(0, tileList.Count);

        var tile = tileList[tileInd];
        tileList.RemoveAt(tileInd);

        return tile;
    }
}

[Serializable]
public class TilesetEntry { 

    public TilePiece tile;
    public int count;
}

