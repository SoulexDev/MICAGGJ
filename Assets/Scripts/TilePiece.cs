
using UnityEngine;
using System.Collections.Generic;

public class TilePiece : MonoBehaviour
{
    public bool[] doors = new bool[4];
    public List<GameObject> availableEntities;

    public float enemyChance = 0.25f;
}
