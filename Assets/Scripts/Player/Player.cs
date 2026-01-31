using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public PlayerController playerController;

    private void Awake()
    {
        Instance = this;
    }
}