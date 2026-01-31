using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    public PlacedTile[][] tiles;

    public bool test;

    public int gridWidth = 9;
    public int gridHeight = 9;

    public float roomSize = 16f;

    public List<PlacedTile> openTiles;

    public RoomTileset Tileset;

    public GameObject TilesRoot;

    void Update()
    {
        if(test)
        {
            test = false;
            RunGenerator();
        }
    }

    void RunGenerator()
    {
        openTiles = new List<PlacedTile>();

        if(TilesRoot != null)
        {
            DestroyImmediate(TilesRoot);
        }

        TilesRoot = new GameObject("Room Tiles");

        tiles = new PlacedTile[gridHeight][];

        for(int i = 0; i < gridHeight; i++)
        {
            tiles[i] = new PlacedTile[gridWidth];
        }

        int wCenter = gridWidth / 2;

        PlacePiece(Tileset.startingTile, wCenter, 0, 0);

        Tileset.BuildTileList();

        while (openTiles.Count > 0)
        {
            // pick a random tile to continue building from
            int ind = Random.Range(0, openTiles.Count);
            var curTile = openTiles[ind];
            openTiles.RemoveAt(ind);
            for(int i = 0; i < 4; i++)
            {
                if (curTile.doors[i])
                {
                    (int newX, int newY) = MoveInDirection(curTile.x, curTile.y, i);
                    if(IsSpaceOpen(newX, newY))
                    {
                        var newPiece = Tileset.TakePiece();
                        if(newPiece == null)
                        {
                            // we're out of pieces
                            openTiles.Clear();
                            break;
                        }
                        int newRot = Random.Range(0, 4);

                        // we are at door i
                        int oppositeDir = OppositeDir(i);

                        int loopPrevent = 0;

                        while(!newPiece.doors[(8 + oppositeDir - newRot) % 4])
                        {
                            newRot++;
                            loopPrevent++;
                            if(loopPrevent == 4)
                            {
                                Debug.LogError($"Invalid placement! Make sure room {newPiece} has doors!");
                                break;
                            }
                        }

                        PlacePiece(newPiece, newX, newY, newRot);
                    }
                }
            }
        }
    }

    int OppositeDir(int dir)
    {
        switch(dir)
        {
            case 0: return 2;
            case 1: return 3;
            case 2: return 0;
            case 3: return 1;
        }

        return -1;
    }

    (int x, int y) MoveInDirection(int x, int y, int dir)
    {
        switch(dir)
        {
            case 0:
                y += 1;
                break;
            case 1:
                x += 1;
                break;
            case 2:
                y -= 1;
                break;
            case 3:
                x -= 1;
                break;
        }

        return (x, y);
    }

    bool IsSpaceOpen(int x, int y)
    {

        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
        {
            return false;
        }

        return tiles[x][y] == null;


    }

    void PlacePiece(TilePiece piece, int x, int y, int rot)
    {
        var placedPrefab = Instantiate(piece.roomPrefab);
        placedPrefab.transform.position = new Vector3(x * roomSize, 0, y * roomSize);
        placedPrefab.transform.parent = TilesRoot.transform;
        placedPrefab.transform.rotation = Quaternion.Euler(0, 90 * rot, 0);
        var placed = new PlacedTile(placedPrefab, piece.doors, x, y, rot);
        placed.x = x;
        placed.y = y;
        placed.rot = rot;

        tiles[x][y] = placed;

        openTiles.Add(placed);

    }
}