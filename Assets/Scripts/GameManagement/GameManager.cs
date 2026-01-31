using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static LayerMask playerRayIgnoreMask;
    public static LayerMask audioOcclusionMask;

    private MapGenerator generator;
    private Player player;

    private void Awake()
    {
        playerRayIgnoreMask = ~LayerMask.GetMask("Player", "Ignore Raycast", "Ignore Player");
        audioOcclusionMask = ~LayerMask.GetMask("Player", "Enemy", "Ignore Raycast");
    }

    private void Start()
    {

        generator = Instantiate(Resources.Load<MapGenerator>("Map Generator 1"));

        generator.RunGenerator();

        player = Instantiate(Resources.Load<Player>("Player"));
        player.transform.position = generator.startingRoom.roomObject.transform.position;
    }


}