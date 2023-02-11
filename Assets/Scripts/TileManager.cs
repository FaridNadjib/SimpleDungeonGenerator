using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
/// <summary>
/// This class represents a tile from the dungeon map. Containing floor, door, wall and ceiling prefabs and builds a random tile out of it.
/// Further more it calls the randomisation option for all of those tile parts.
/// </summary>
public class TileManager : MonoBehaviour
{
    [SerializeField] bool randomFloorRotation;
    [SerializeField, Tooltip("Keep empty if floor position equals the transforms position.")] Transform floorPosition;
    [SerializeField] Prefab[] floorTiles;
    [SerializeField] Transform[] doorAndWallPositions;
    [SerializeField] Prefab[] doors;
    [SerializeField] Prefab[] walls;
    [SerializeField] bool randomCeilingRotation;
    [SerializeField] Transform ceilingPosition;
    [SerializeField] Prefab[] ceilings;

    /// <summary>
    /// 4 for each direction. True = spawn door, false = wall, null = neither.
    /// </summary>
    private bool?[] spawnDoors = new bool?[4];
    private DungeonSettings settings;

    /// <summary>
    /// Randomizes the tile based on the different prefab pools.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void RandomizeTile(TileInfo[,] map, int x, int y, DungeonSettings settings)
    {
        // Make reference to the settings.
        this.settings = settings;

        if(settings.SpawnFloors)
            SpawnFloor();
        SpawnDoors(map, x, y);
        if (settings.SpawnCeilings)
            SpawnCeiling();
    }

    public void RandomizeMapBorder(TileInfo tile, DungeonSettings settings)
    {
        // Make reference to the settings.
        this.settings = settings;

        if (settings.SpawnBordersFloors)
            SpawnFloor();
        if (settings.SpawnBordersWalls)
            SpawnMapBorders(tile);
        if (settings.SpawnBordersCeilings)
            SpawnCeiling();
    }

    private void SpawnMapBorders(TileInfo tile)
    {
        if (walls.Length == 0)
            return;

        // Spawn the map borders based on the tile informations.
        for (int i = 0; i < tile.SpawnMapBorder.Length; i++)
        {
            if (tile.SpawnMapBorder[i])
            {
                Instantiate(Prefab.GetPrefabByChance(walls), doorAndWallPositions[i].position, doorAndWallPositions[i].rotation, transform);
            }
        }

    }

    /// <summary>
    /// Spawn a random floor prefab from the tile.
    /// </summary>
    private void SpawnFloor()
    {
        if (floorTiles.Length == 0)
            return;

        Vector3 spawnPosition = floorPosition != null ? floorPosition.position : transform.position;
        if (randomFloorRotation)
        { 
            int rnd = Random.Range(0, 4);
            var roomPart = Instantiate(Prefab.GetPrefabByChance(floorTiles), spawnPosition, Quaternion.identity, transform).transform;
            if (settings.RandomizeRooms && roomPart.TryGetComponent(out IRandomizable randomController))
                randomController.Randomize();
            roomPart.transform.Rotate(Vector3.up, 90 * rnd);
            
        }
        else
        {
            var roomPart = Instantiate(Prefab.GetPrefabByChance(floorTiles), spawnPosition, Quaternion.identity, transform);
            if (settings.RandomizeRooms && roomPart.TryGetComponent(out IRandomizable randomController))
                randomController.Randomize();
            
        }
    }

    /// <summary>
    /// Spawns the doors based on the connections of the specific tile. And copies the door informations to the dungeon map array.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void SpawnDoors(TileInfo[,] map, int x, int y)
    {
        if (doors.Length == 0)//Todo: check for walls != null
            return;
        // Which directions doors need to be spawned in? Consider already spawned neighbour cells as well.
        CalculateSpawnIndices(map[x,y].BitmaskIndex, map, x, y);
        
        // Spawn the doors and walls.
        for (int i = 0; i < spawnDoors.Length; i++)
        {
            if (settings.SpawnDoors && spawnDoors[i] == true && !map[x, y].DoorsSpawned[i])
            {
                var roomPart = Instantiate(Prefab.GetPrefabByChance(doors), doorAndWallPositions[i].position, doorAndWallPositions[i].rotation,transform);
                if (settings.RandomizeRooms && roomPart.TryGetComponent(out IRandomizable randomController))
                    randomController.Randomize();

            }
            else if (settings.SpawnWalls && spawnDoors[i] == false)
            {
                var roomPart = Instantiate(Prefab.GetPrefabByChance(walls), doorAndWallPositions[i].position, doorAndWallPositions[i].rotation, transform);
                if (settings.RandomizeRooms && roomPart.TryGetComponent(out IRandomizable randomController))
                    randomController.Randomize();
            }
        }
        CopySpawnInformationsToDungeonMap(map, x, y);
    }

    /// <summary>
    /// Spawn a random ceiling prefab from the tile.
    /// </summary>
    private void SpawnCeiling()
    {
        if (ceilings.Length == 0)
            return;

        if (randomCeilingRotation)
        {
            int rnd = Random.Range(0, 4);
            var roomPart = Instantiate(Prefab.GetPrefabByChance(ceilings), ceilingPosition.position, ceilingPosition.rotation, transform); 
            if (settings.RandomizeRooms && roomPart.TryGetComponent(out IRandomizable randomController))
                randomController.Randomize();
            roomPart.transform.Rotate(Vector3.up, 90 * rnd);
        }
        else
        {
            var roomPart = Instantiate(Prefab.GetPrefabByChance(ceilings), ceilingPosition.position, ceilingPosition.rotation, transform);
            if (settings.RandomizeRooms && roomPart.TryGetComponent(out IRandomizable randomController))
                randomController.Randomize();
        }
    }

    /// <summary>
    /// Copies the informations about the spawned doors to the dungeon map array. To avoid doubled spawned doors.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void CopySpawnInformationsToDungeonMap(TileInfo[,] map, int x, int y)
    {
        // Copy spawned doors informations to the original tile of the map.
        for (int i = 0; i < spawnDoors.Length; i++)
            if (spawnDoors[i] == true)
                map[x, y].DoorsSpawned[i] = true;

        // Copy spawned informations to each adjacent maptile, so nothing is spawned twice.
        // North:
        if (spawnDoors[0] == true && ValidCoordinates(x,y+1,map))
            map[x, y + 1].DoorsSpawned[2] = true;
        // East:
        if (spawnDoors[1] == true && ValidCoordinates(x + 1, y, map))
            map[x + 1, y].DoorsSpawned[3] = true;
        // South:
        if (spawnDoors[2] == true && ValidCoordinates(x, y - 1, map))
            map[x, y - 1].DoorsSpawned[0] = true;
        // West:
        if (spawnDoors[3] == true && ValidCoordinates(x - 1, y, map))
            map[x - 1, y].DoorsSpawned[1] = true;
    }

    private bool ValidCoordinates(int x, int y, TileInfo[,] map) => x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1);

    /// <summary>
    /// Marks the connections of each tile either as door or wall, based on its bitmask index.
    /// </summary>
    /// <param name="bitmaskIndex"></param>
    /// <param name="map"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void CalculateSpawnIndices(int bitmaskIndex, TileInfo[,] map, int x, int y)
    {
        // ToDO: write helper method to make this smaller.
        switch ((Direction)bitmaskIndex)
        {
            case Direction.North:
                SetDoorIndexByBuildPriority(map, x, y, Direction.North);
                SetWallSpawn(Direction.East);
                SetWallSpawn(Direction.South);
                SetWallSpawn(Direction.West);
                break;
            case Direction.East:
                SetDoorIndexByBuildPriority(map, x, y, Direction.East);
                SetWallSpawn(Direction.North);
                SetWallSpawn(Direction.South);
                SetWallSpawn(Direction.West);
                break;
            case Direction.South:
                SetDoorIndexByBuildPriority(map, x, y, Direction.South);
                SetWallSpawn(Direction.North);
                SetWallSpawn(Direction.East);
                SetWallSpawn(Direction.West);
                break;
            case Direction.West:
                SetDoorIndexByBuildPriority(map, x, y, Direction.West);
                SetWallSpawn(Direction.North);
                SetWallSpawn(Direction.South);
                SetWallSpawn(Direction.East);
                break;
            case Direction.NorthEast:
                SetDoorIndexByBuildPriority(map, x, y, Direction.North);
                SetDoorIndexByBuildPriority(map, x, y, Direction.East);
                SetWallSpawn(Direction.South);
                SetWallSpawn(Direction.West);
                break;
            case Direction.EastSouth:
                SetDoorIndexByBuildPriority(map, x, y, Direction.East);
                SetDoorIndexByBuildPriority(map, x, y, Direction.South);
                SetWallSpawn(Direction.North);
                SetWallSpawn(Direction.West);
                break;
            case Direction.SouthWest:
                SetDoorIndexByBuildPriority(map, x, y, Direction.South);
                SetDoorIndexByBuildPriority(map, x, y, Direction.West);
                SetWallSpawn(Direction.North);
                SetWallSpawn(Direction.East);
                break;
            case Direction.WestNorth:
                SetDoorIndexByBuildPriority(map, x, y, Direction.West);
                SetDoorIndexByBuildPriority(map, x, y, Direction.North);
                SetWallSpawn(Direction.East);
                SetWallSpawn(Direction.South);
                break;
            case Direction.NorthSouth:
                SetDoorIndexByBuildPriority(map, x, y, Direction.North);
                SetDoorIndexByBuildPriority(map, x, y, Direction.South);
                SetWallSpawn(Direction.East);
                SetWallSpawn(Direction.West);
                break;
            case Direction.EastWest:
                SetDoorIndexByBuildPriority(map, x, y, Direction.East);
                SetDoorIndexByBuildPriority(map, x, y, Direction.West);
                SetWallSpawn(Direction.North);
                SetWallSpawn(Direction.South);
                break;
            case Direction.NorthEastSouth:
                SetDoorIndexByBuildPriority(map, x, y, Direction.North);
                SetDoorIndexByBuildPriority(map, x, y, Direction.East);
                SetDoorIndexByBuildPriority(map, x, y, Direction.South);
                SetWallSpawn(Direction.West);
                break;
            case Direction.EastSouthWest:
                SetDoorIndexByBuildPriority(map, x, y, Direction.East);
                SetDoorIndexByBuildPriority(map, x, y, Direction.South);
                SetDoorIndexByBuildPriority(map, x, y, Direction.West);
                SetWallSpawn(Direction.North);
                break;
            case Direction.SouthWestNorth:
                SetDoorIndexByBuildPriority(map, x, y, Direction.South);
                SetDoorIndexByBuildPriority(map, x, y, Direction.West);
                SetDoorIndexByBuildPriority(map, x, y, Direction.North);
                SetWallSpawn(Direction.East);
                break;
            case Direction.WestNorthEast:
                SetDoorIndexByBuildPriority(map, x, y, Direction.West);
                SetDoorIndexByBuildPriority(map, x, y, Direction.North);
                SetDoorIndexByBuildPriority(map, x, y, Direction.East);
                SetWallSpawn(Direction.South);
                break;
            case Direction.NorthEastSouthWest:
                SetDoorIndexByBuildPriority(map, x, y, Direction.North);
                SetDoorIndexByBuildPriority(map, x, y, Direction.East);
                SetDoorIndexByBuildPriority(map, x, y, Direction.South);
                SetDoorIndexByBuildPriority(map, x, y, Direction.West);
                break;
            default:
                break;
        }       
    }

    /// <summary>
    /// Uses the build priority of each map cell and its neighbours to mark directions as door spawn points.
    /// </summary>
    /// <param name="map"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="direction"></param>
    private void SetDoorIndexByBuildPriority(TileInfo[,] map, int x, int y, Direction direction)
    {
        if (direction == Direction.North && ValidCoordinates(x, y + 1, map))
        {
            if (map[x, y].BuildPriority >= map[x, y + 1].BuildPriority)
                spawnDoors[0] = true;
        }
        else if (direction == Direction.East && ValidCoordinates(x + 1, y, map))
        {
            if (map[x, y].BuildPriority >= map[x + 1, y].BuildPriority) spawnDoors[1] = true;
        }
        else if (direction == Direction.South && ValidCoordinates(x, y - 1, map))
        {
            if (map[x, y].BuildPriority >= map[x, y - 1].BuildPriority) spawnDoors[2] = true;
        }
        else if (direction == Direction.West && ValidCoordinates(x - 1, y, map))
        {
            if (map[x, y].BuildPriority >= map[x - 1, y].BuildPriority) spawnDoors[3] = true;
        }
    }

    /// <summary>
    /// Set the direction to be a wall.
    /// </summary>
    /// <param name="direction">The direction to be a wall.</param>
    private void SetWallSpawn(Direction direction)
    {
        if (direction == Direction.North)
            spawnDoors[0] = false;
        else if(direction == Direction.East)
            spawnDoors[1] = false;
        else if (direction == Direction.South)
            spawnDoors[2] = false;
        else if (direction == Direction.West)
            spawnDoors[3] = false;
    }

    
}

/// <summary>
/// Serializable struct containing the gameobject and its spawn chance plus method to get a random object out of a Prefab array.
/// </summary>
[System.Serializable]
public struct Prefab
{
    [SerializeField] GameObject prefab;
    [Range(0f, 1f)] public float spawnChance;

    /// <summary>
    /// Gives a random prefab out of a prefab array.
    /// </summary>
    /// <param name="objectPool">The prefab pool.</param>
    /// <returns>The random prefab.</returns>
    public static GameObject GetPrefabByChance(Prefab[] objectPool)
    {
        for (int i = 0; i < objectPool.Length; i++)
            if (Random.value <= objectPool[i].spawnChance) return objectPool[i].prefab;

        return objectPool[0].prefab;
    }
}

/// <summary>
/// All possible directions based on a 4 way bitmask calculation.
/// </summary>
public enum Direction
{
    North = 1, East = 2, South = 4, West = 8,
    NorthEast = 3, EastSouth = 6, SouthWest = 12, WestNorth = 9,
    NorthSouth = 5, EastWest = 10,
    NorthEastSouth = 7, EastSouthWest = 14, SouthWestNorth = 13, WestNorthEast = 11,
    NorthEastSouthWest = 15
}

