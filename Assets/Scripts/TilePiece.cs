
using UnityEngine;


[CreateAssetMenu(fileName = "Tile", menuName = "My Assets/Tile Piece")]
public class TilePiece : ScriptableObject
{
    public bool[] doors = new bool[4];
    public GameObject roomPrefab;
}
