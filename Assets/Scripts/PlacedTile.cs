using UnityEngine;

public class PlacedTile
{
    public TilePiece piece;
    public int rot;

    public int x, y;

    public bool[] doors;

    public GameObject roomObject;

    public PlacedTile(GameObject obj, bool[] doors, int x, int y, int rot)
    {
        roomObject = obj;
        this.x = x;
        this.y = y;
        this.rot = rot;
        this.doors = new bool[doors.Length];
        for (int i = 0; i < doors.Length; i++)
        {
            this.doors[i] = doors[(8 + i - rot) % 4];
        }
    }
}