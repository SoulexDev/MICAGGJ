using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static LayerMask playerRayIgnoreMask;
    public static LayerMask monsterRayIgnoreMask;
    public static LayerMask policeRayIgnoreMask;
    public static LayerMask civilianRayIgnoreMask;
    public static LayerMask audioOcclusionMask;

    private MapGenerator generator;
    private Player player;

    private NavMeshSurface navSurface;

    public bool testScene = false;

    private void Awake()
    {
        playerRayIgnoreMask = ~LayerMask.GetMask("Player", "Ignore Raycast", "Ignore Player");
        monsterRayIgnoreMask = ~LayerMask.GetMask("Monster", "Ignore Raycast", "Ignore Monster");
        policeRayIgnoreMask = ~LayerMask.GetMask("Police", "Ignore Raycast", "Ignore Police");
        audioOcclusionMask = ~LayerMask.GetMask("Player", "Enemy", "Ignore Raycast");
    }

    private void Start()
    {   
        if(!testScene)
        {
            navSurface = FindFirstObjectByType<NavMeshSurface>();

            generator = Instantiate(Resources.Load<MapGenerator>("Map Generator"));

            generator.RunGenerator();
            navSurface.BuildNavMesh();

            generator.ProcessEnemySpawns();

            player = Instantiate(Resources.Load<Player>("Player"));
            player.transform.position = generator.startingRoom.tilePiece.transform.position;
            CharacterManager.Instance.AddCharacter(player.GetComponentInChildren<Character>());
        }


    }


}