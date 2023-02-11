using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class TileSpawner
{
    //Todo: This method is not really needed anymore.
    public static void RotateTile(GameObject tile, int bitmaskIndex)
    {
        switch ((Direction)bitmaskIndex)
        {
            case Direction.North:
                tile.transform.Rotate(Vector3.up, 0f);
                break;
            case Direction.East:
                tile.transform.Rotate(Vector3.up, 90f);
                break;
            case Direction.South:
                tile.transform.Rotate(Vector3.up, 180f);
                break;
            case Direction.West:
                tile.transform.Rotate(Vector3.up, 270f);
                break;
            case Direction.NorthEast:
                tile.transform.Rotate(Vector3.up, 0f);
                break;
            case Direction.EastSouth:
                tile.transform.Rotate(Vector3.up, 90f);
                break;
            case Direction.SouthWest:
                tile.transform.Rotate(Vector3.up, 180f);
                break;
            case Direction.WestNorth:
                tile.transform.Rotate(Vector3.up, 270f);
                break;
            case Direction.NorthSouth:
                tile.transform.Rotate(Vector3.up, 0f);
                break;
            case Direction.EastWest:
                tile.transform.Rotate(Vector3.up, 90f);
                break;
            case Direction.NorthEastSouth:
                tile.transform.Rotate(Vector3.up, 0f);
                break;
            case Direction.EastSouthWest:
                tile.transform.Rotate(Vector3.up, 90f);
                break;
            case Direction.SouthWestNorth:
                tile.transform.Rotate(Vector3.up, 180f);
                break;
            case Direction.WestNorthEast:
                tile.transform.Rotate(Vector3.up, 270f);
                break;
            case Direction.NorthEastSouthWest:
                tile.transform.Rotate(Vector3.up, 0f);
                break;
            default:
                break;
        }
    }
}
