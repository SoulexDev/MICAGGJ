using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum MapgenRoomState
{
    Available,
    OutOfBounds,
    RoomWithPassage,
    RoomWithoutPassage,
    
}

[ExecuteInEditMode]
public class MapGenerator : MonoBehaviour
{
    public PlacedTile[][] tiles;

    public bool test;

    public int gridWidth = 9;
    public int gridHeight = 9;

    public float roomSize = 16f;

    public List<PlacedTile> openTiles;

    public List<(PlacedTile, int)> closedDoors;

    public List<(PlacedTile tile, GameObject enemy)> entitySpawns;

    public RoomTileset Tileset;

    public GameObject TilesRoot;

    public GameObject EnemiesRoot;

    public PlacedTile startingRoom;

    private GameObject closedDoor;

    void Update()
    {
        if(test)
        {
            test = false;
            RunGenerator();
            var nav = FindFirstObjectByType<NavMeshSurface>() ?? new GameObject("NavMeshSurface").AddComponent<NavMeshSurface>();
            nav.BuildNavMesh();
            ProcessEnemySpawns();
        }
    }

    public void RunGenerator()
    {
        openTiles = new List<PlacedTile>();
        entitySpawns = new List<(PlacedTile, GameObject)>();
        closedDoors = new List<(PlacedTile, int)>();

        closedDoor = Resources.Load<GameObject>("Closed Door");

        if(TilesRoot != null)
        {
            DestroyImmediate(TilesRoot);
        }

        if(EnemiesRoot != null)
        {
            DestroyImmediate(EnemiesRoot);
        }

        TilesRoot = new GameObject("Room Tiles");
        EnemiesRoot = new GameObject("Enemies");

        tiles = new PlacedTile[gridHeight][];

        for(int i = 0; i < gridHeight; i++)
        {
            tiles[i] = new PlacedTile[gridWidth];
        }

        int wCenter = gridWidth / 2;

        startingRoom = PlacePiece(Tileset.startingTile, wCenter, 0, 0);
        var startChoice = Tileset.startingTile.availableEntities[Random.Range(0, Tileset.startingTile.availableEntities.Count)];
        entitySpawns.Add((startingRoom, startChoice));

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
                    var state = GetRoomState(newX, newY, i);
                    if (state == MapgenRoomState.Available)
                    {
                        var newPiece = Tileset.TakePiece();
                        if (newPiece == null)
                        {
                            // we're out of pieces
                            closedDoors.Add((curTile, i));
                            continue;
                        }
                        int newRot = Random.Range(0, 4);

                        // we are at door i
                        int oppositeDir = OppositeDir(i);

                        int loopPrevent = 0;

                        while (!newPiece.doors[(8 + oppositeDir - newRot) % 4])
                        {
                            newRot++;
                            loopPrevent++;
                            if (loopPrevent == 4)
                            {
                                Debug.LogError($"Invalid placement! Make sure room {newPiece} has doors!");
                                break;
                            }
                        }

                        var newTile = PlacePiece(newPiece, newX, newY, newRot);

                        if (newPiece.availableEntities.Count > 0 && Random.Range(0, 1f) < newPiece.enemyChance)
                        {
                            Debug.Log("Queuing enemy spawn");
                            var choice = newPiece.availableEntities[Random.Range(0, newPiece.availableEntities.Count)];
                            entitySpawns.Add((newTile, choice));
                        }
                    }
                    else if (state == MapgenRoomState.OutOfBounds || state == MapgenRoomState.RoomWithoutPassage)
                    {
                        {
                            closedDoors.Add((curTile, i));
                        }
                    }
                }
            }
        }

        while (closedDoors.Count > 0)
        {
            (PlacedTile tile, int door) = closedDoors[0];
            closedDoors.RemoveAt(0);

            float xPos = tile.tilePiece.transform.position.x;
            float zPos = tile.tilePiece.transform.position.z;

            switch (door)
            {
                case 0:
                    zPos += roomSize / 2;
                    break;
                case 1:
                    xPos += roomSize / 2;
                    break;
                case 2:
                    zPos -= roomSize / 2;
                    break;
                case 3:
                    xPos -= roomSize / 2;
                    break;
            }

            var closed = Instantiate(closedDoor, new Vector3(xPos, 0, zPos), Quaternion.Euler(0, 90 * (door - 1), 0));
            closed.transform.parent = tile.tilePiece.transform;
        }
    }

    // run after the navmesh builds?
    public void ProcessEnemySpawns()
    {
        while(entitySpawns.Count > 0)
        {
            var next = entitySpawns[0];
            entitySpawns.RemoveAt(0);

            var samplePos = next.tile.tilePiece.transform.position;
            //samplePos += new Vector3(Random.Range(-roomSize / 2, roomSize / 2), 0, Random.Range(-roomSize / 2, roomSize / 2));

            if (NavMesh.SamplePosition(samplePos, out var hit, 20f, 1))
            {
                var enemy = Instantiate(next.enemy, hit.position + Vector3.up * 0.8f, Quaternion.identity);
                enemy.transform.parent = EnemiesRoot.transform;

                if (enemy.transform.TryGetComponent(out Character c))
                    CharacterManager.Instance.AddCharacter(c);
            }
            else
            {
                Debug.Log("Enemy spawn skipped!");
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

    bool GetState(int x, int y)
    {

        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
        {
            return false;
        }

        return tiles[x][y] == null;


    }

    PlacedTile PlacePiece(TilePiece piece, int x, int y, int rot)
    {
        var placedPrefab = Instantiate(piece, new Vector3(x * roomSize, 0, y * roomSize), Quaternion.Euler(0, 90f * rot, 0));
        placedPrefab.transform.parent = TilesRoot.transform;
        var placed = new PlacedTile(placedPrefab, piece.doors, x, y, rot);
        placed.x = x;
        placed.y = y;
        placed.rot = rot;

        tiles[x][y] = placed;

        openTiles.Add(placed);

        var spawnMarkers = placedPrefab.GetComponentsInChildren<SpawnMarker>();
        foreach (var spawnMarker in spawnMarkers)
        {
            spawnMarker.ProcessSpawn();
        }

        return placed;

    }

    MapgenRoomState GetRoomState(int x, int y, int fromDir)
    {
        if(x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
        {
            return MapgenRoomState.OutOfBounds;
        }
        var tile = tiles[x][y];
        int oppositeDir = OppositeDir(fromDir);

        if (tile == null)
        {
            return MapgenRoomState.Available;
        }
        else if (tile.doors[oppositeDir])
        {
            return MapgenRoomState.RoomWithPassage;
        }
        else
        {
            return MapgenRoomState.RoomWithoutPassage;
        }
    }
}