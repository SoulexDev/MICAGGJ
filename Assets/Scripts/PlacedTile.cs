using UnityEngine;

public class PlacedTile
{
    public int rot;

    public int x, y;

    public bool[] doors;

    public TilePiece tilePiece;

    public PlacedTile(TilePiece obj, bool[] doors, int x, int y, int rot)
    {
        tilePiece = obj;
        this.x = x;
        this.y = y;
        this.rot = rot;
        this.doors = new bool[doors.Length];
        for (int i = 0; i < doors.Length; i++)
        {
            this.doors[i] = doors[(8 + i - rot) % 4];
        }
        tilePiece.doors = doors;
    }
}