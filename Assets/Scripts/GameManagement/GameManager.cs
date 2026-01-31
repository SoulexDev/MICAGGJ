using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static LayerMask playerRayIgnoreMask;

    private void Awake()
    {
        playerRayIgnoreMask = ~LayerMask.GetMask("Player", "Ignore Raycast", "Ignore Player");
    }
}