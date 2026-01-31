using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static LayerMask playerRayIgnoreMask;

    private MapGenerator generator;
    private Player player;

    private NavMeshSurface navSurface;

    private void Awake()
    {
        playerRayIgnoreMask = ~LayerMask.GetMask("Player", "Ignore Raycast", "Ignore Player");
    }

    private void Start()
    {
        navSurface = FindFirstObjectByType<NavMeshSurface>();

        generator = Instantiate(Resources.Load<MapGenerator>("Map Generator 1"));

        generator.RunGenerator();
        navSurface.BuildNavMesh();

        player = Instantiate(Resources.Load<Player>("Player"));
        player.transform.position = generator.startingRoom.roomObject.transform.position;
    }


}