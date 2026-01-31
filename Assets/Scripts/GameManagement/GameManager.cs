using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static LayerMask playerRayIgnoreMask;
    public static LayerMask audioOcclusionMask;

    private void Awake()
    {
        playerRayIgnoreMask = ~LayerMask.GetMask("Player", "Ignore Raycast", "Ignore Player");
        audioOcclusionMask = ~LayerMask.GetMask("Player", "Enemy", "Ignore Raycast");
    }
}