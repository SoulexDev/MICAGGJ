using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static LayerMask playerRayIgnoreMask;
    public static LayerMask audioOcclusionMask;

    private MapGenerator generator;
    private Player player;

    private NavMeshSurface navSurface;

    public bool testScene = false;

    private void Awake()
    {
        playerRayIgnoreMask = ~LayerMask.GetMask("Player", "Ignore Raycast", "Ignore Player");
        audioOcclusionMask = ~LayerMask.GetMask("Player", "Enemy", "Ignore Raycast");
    }

    private void Start()
    {   
        if(!testScene)
        {
            navSurface = FindFirstObjectByType<NavMeshSurface>();

            generator = Instantiate(Resources.Load<MapGenerator>("Map Generator 1"));

            generator.RunGenerator();
            navSurface.BuildNavMesh();

            generator.ProcessEnemySpawns();

            player = Instantiate(Resources.Load<Player>("Player"));
            player.transform.position = generator.startingRoom.tilePiece.transform.position;
        }


    }


}