using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class saves some globally used settings of the generated dungeon.
/// </summary>
[CreateAssetMenu(fileName = "New Dungeon Settings", menuName = "SOs/Create new DungeonSettings")]
public class DungeonSettings : ScriptableObject
{
    bool spawnMapBorders;
    bool spawnBordersFloors;
    bool spawnBordersWalls;
    bool spawnBordersCeilings;
    bool spawnFloors;
    bool spawnDoors;
    bool spawnWalls;
    bool spawnCeilings;
    bool randomizeRooms;
    float globalFluidModifier;
    float globalTrapModifier;
    float globalRewardModifier;
    float globalDecorationModifier;
    float globalParticleModifier;
    float globalEnemyModifier;
    float globalMiscModifier;

    public bool SpawnMapBorders { get => spawnMapBorders; set => spawnMapBorders = value; }
    public bool SpawnBordersFloors { get => spawnBordersFloors; set => spawnBordersFloors = value; }
    public bool SpawnBordersWalls { get => spawnBordersWalls; set => spawnBordersWalls = value; }
    public bool SpawnBordersCeilings { get => spawnBordersCeilings; set => spawnBordersCeilings = value; }
    public bool SpawnFloors { get => spawnFloors; set => spawnFloors = value; }
    public bool SpawnDoors { get => spawnDoors; set => spawnDoors = value; }
    public bool SpawnWalls { get => spawnWalls; set => spawnWalls = value; }
    public bool SpawnCeilings { get => spawnCeilings; set => spawnCeilings = value; }
    public bool RandomizeRooms { get => randomizeRooms; set => randomizeRooms = value; }
    public float GlobalFluidModifier { get => globalFluidModifier; set => globalFluidModifier = value; }
    public float GlobalTrapModifier { get => globalTrapModifier; set => globalTrapModifier = value; }
    public float GlobalRewardModifier { get => globalRewardModifier; set => globalRewardModifier = value; }
    public float GlobalDecorationModifier { get => globalDecorationModifier; set => globalDecorationModifier = value; }
    public float GlobalParticleModifier { get => globalParticleModifier; set => globalParticleModifier = value; }
    public float GlobalEnemyModifier { get => globalEnemyModifier; set => globalEnemyModifier = value; }
    public float GlobalMiscModifier { get => globalMiscModifier; set => globalMiscModifier = value; }

    public void SaveSettings(bool mapBorders, bool borderFloors, bool borderWalls, bool borderCeilings, 
        bool floors, bool doors, bool walls, bool ceilings, bool randomRooms,
        float fluid, float trap, float reward, float deco, float particle, float enemy, float misc)
    {
        // Set bools.
        SpawnMapBorders = mapBorders;
        spawnBordersFloors = borderFloors;
        spawnBordersWalls = borderWalls;
        spawnBordersCeilings = borderCeilings;
        SpawnFloors = floors;
        SpawnDoors = doors;
        SpawnWalls = walls;
        SpawnCeilings = ceilings;
        RandomizeRooms = randomRooms;

        // Set global multipliers.
        GlobalFluidModifier = fluid;
        GlobalTrapModifier = trap;
        GlobalRewardModifier = reward;
        GlobalDecorationModifier = deco;
        GlobalParticleModifier = particle;
        GlobalEnemyModifier = enemy;
        GlobalMiscModifier = misc;
    }
}
