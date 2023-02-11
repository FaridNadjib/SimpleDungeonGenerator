using UnityEngine;

[CreateAssetMenu(fileName = "New Tile Settings", menuName = "SOs/Legacy/SimpleDungeonTileSettings")]
public class DungeonTileSettings : ScriptableObject
{
    /*Del me, was the first attempt and needs kinda premade rooms already.
     *
     *
     *
     *
     *
     *
     *
     */

    [SerializeField] private GameObject[] oneDoorTiles;
    [SerializeField] private GameObject[] twoDoorTilesCorner;
    [SerializeField] private GameObject[] twoDoorTilesStraight;
    [SerializeField] private GameObject[] threeDoorTiles;
    [SerializeField] private GameObject[] fourDoorTiles;
    private GameObject tmp;

    public GameObject GetDungeonTile(int bitmaskIndex)
    {
        //GameObject tmp = new GameObject();

        switch ((Direction)bitmaskIndex)
        {
            case Direction.North:
            case Direction.East:
            case Direction.South:
            case Direction.West:
                tmp = oneDoorTiles[Random.Range(0, oneDoorTiles.Length)];
                break;

            case Direction.NorthEast:
            case Direction.EastSouth:
            case Direction.SouthWest:
            case Direction.WestNorth:
                tmp = twoDoorTilesCorner[Random.Range(0, twoDoorTilesCorner.Length)];
                break;

            case Direction.NorthSouth:
            case Direction.EastWest:
                tmp = twoDoorTilesStraight[Random.Range(0, twoDoorTilesStraight.Length)];
                break;

            case Direction.NorthEastSouth:
            case Direction.EastSouthWest:
            case Direction.SouthWestNorth:
            case Direction.WestNorthEast:
                tmp = threeDoorTiles[Random.Range(0, threeDoorTiles.Length)];
                break;

            case Direction.NorthEastSouthWest:
                tmp = fourDoorTiles[Random.Range(0, fourDoorTiles.Length)];
                break;

            default:
                break;
        }

        return tmp;
    }
}