using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// A class to create a simple 4 directions bitmask using dungeon.
/// </summary>
public class SimpleDungeon : MonoBehaviour
{
    #region DungeonSettings
    [Header("Base Dungeon Settings:")]
    [SerializeField, Min(1)] int width;
    [SerializeField, Min(1)] int height;
    [SerializeField] int maxGrowSteps;
    [SerializeField] MinMaxValue<int> dungeonTilesAmount = new MinMaxValue<int>(1,1000,20,200);
    int maxRepeatTries = 3;
    [SerializeField] int seed;
    [SerializeField] bool randomStartPosition;
    [SerializeField] Vector2Int startPos;
    [SerializeField] GrowChances growChances;

    [Space, Header("Dungeon Settings:")]
    [SerializeField] BiomType biom;
    [SerializeField] TileAtlas tileAtlas;
    [SerializeField] GameObject starterPrefab;
    [SerializeField] Transform holder;

    [SerializeField] bool useSpecialRoomChances;
    [SerializeField, Range(0f, 1f), ShowIf("useSpecialRoomChances")] float special1DRoomChance;
    [SerializeField, Range(0f, 1f), ShowIf("useSpecialRoomChances")] float special2DSRoomChance;

    [Space, Header("Global Dungeon Settings:")]
    [SerializeField] DungeonSettings settingsAsset;
    [SerializeField] bool spawnMapBorders = true;
    [SerializeField, ShowIf("spawnMapBorders")] bool spawnBordersFloors = true;
    [SerializeField, ShowIf("spawnMapBorders")] bool spawnBordersWalls = true;
    [SerializeField, ShowIf("spawnMapBorders")] bool spawnBordersCeilings = true;
    [SerializeField] bool spawnFloors = true;
    [SerializeField] bool spawnDoors = true;
    [SerializeField] bool spawnWalls = true;
    [SerializeField] bool spawnCeilings = true;
    [SerializeField] bool randomizeRooms = true;
    [SerializeField, Range(0f,3f)] float globalFluidModifier = 1f;
    [SerializeField, Range(0f,3f)] float globalTrapModifier = 1f;
    [SerializeField, Range(0f,3f)] float globalRewardModifier = 1f;
    [SerializeField, Range(0f,3f)] float globalDecorationModifier = 1f;
    [SerializeField, Range(0f,3f)] float globalParticleModifier = 1f;
    [SerializeField, Range(0f,3f)] float globalEnemyModifier = 1f;
    [SerializeField, Range(0f,3f)] float globalMiscModifier = 1f;
    #endregion

    #region ComponentsAndUtility
    [SerializeField, Header("Misc and Utility")] Grid grid;
    TileInfo[,] map;
    int currentDungeonTiles;
    int currentTries;
    #endregion

    private void Awake()
    {
        // ToDO: set the cell size in inspector based on the maptile size.
        grid = GetComponent<Grid>();
    }

    /// <summary>
    /// Generates a simple dungeon based on a 4 directions recursive algorithm.
    /// </summary>
    [ContextMenu("GenerateSimpleDungeon")]
    public void GenerateSimpleDungeon()
    {
        // Destroys all previously instantiated dungeon tiles.
        for (int i = holder.childCount; i > 0; i--)
            DestroyImmediate(holder.GetChild(0).gameObject);
        currentDungeonTiles = 0;
        currentTries = 0;

        // Store global settings.
        settingsAsset.SaveSettings(spawnMapBorders, spawnBordersFloors, spawnBordersWalls, spawnBordersCeilings, 
            spawnFloors, spawnDoors, spawnWalls, spawnCeilings, randomizeRooms,
            globalFluidModifier,globalTrapModifier,globalRewardModifier, globalDecorationModifier,globalParticleModifier,globalEnemyModifier,globalMiscModifier);

        // Init the map.
        InitMap();

        // Init the seed.
        Random.InitState(seed);

        // Get and mark starting position.
        startPos = randomStartPosition ? new Vector2Int(Random.Range(1, width -1), Random.Range(1, height-1)) : startPos;
        Instantiate(starterPrefab, grid.CellToWorld(new Vector3Int(startPos.x, 0, startPos.y)), Quaternion.identity, holder);
        map[startPos.x, startPos.y].IsStartingRoom = true;

        // Start growing the dungeon.
        while (true)
        {
            GrowDungeonRecursive(startPos.x, startPos.y, maxGrowSteps);
            // Check is the generated dungeon has base size.
            if(currentDungeonTiles < dungeonTilesAmount.Min && currentTries < maxRepeatTries)
            {
                InitMap();
                Random.InitState(Random.Range(0,10000));
                GrowDungeonRecursive(startPos.x, startPos.y, maxGrowSteps);
                currentTries++;
                Debug.Log("New try.");
            }
            else
                break;
        }
        
        if(spawnMapBorders) MarkDungeonBorders();
        CalculateBitmaskValues();

        // Spawn the dungeon tiles.
        DrawSimpleDungeon();
    }

    /// <summary>
    /// Grow the dungeon recursivly by marking cells as either dungeoncells or no dungeoncells.
    /// </summary>
    /// <param name="x">X-coordinate of the cell.</param>
    /// <param name="y">Y-coordinate of the cell.</param>
    /// <param name="growSteps">How many times to repeat.</param>
    private void GrowDungeonRecursive(int x, int y, int growSteps)
    {
        // Check for finishing conditions.
        if (!ValidCoordinates(x, y) || growSteps <= 0 || map[x,y].IsDungeonCell != null || currentDungeonTiles >= dungeonTilesAmount.Max)
            return;

        // Mark as dungeoncell.
        map[x, y].IsDungeonCell = true;
        growSteps--;
        currentDungeonTiles++;
   
        // Grow the dungeon in each direction, consider growdirection chances.
        // North:
        if (Random.value < growChances.north)
            GrowDungeonRecursive(x, y + 1, growSteps);
        else if(ValidCoordinates(x, y + 1) && map[x, y + 1].IsDungeonCell == null)
            map[x, y + 1].IsDungeonCell = false;

        // East:
        if (Random.value < growChances.east)
            GrowDungeonRecursive(x + 1, y, growSteps);
        else if(ValidCoordinates(x + 1, y) && map[x + 1, y].IsDungeonCell == null)
            map[x + 1, y].IsDungeonCell = false;

        // South:
        if (Random.value < growChances.south)
            GrowDungeonRecursive(x, y - 1, growSteps);
        else if (ValidCoordinates(x, y - 1) && map[x, y - 1].IsDungeonCell == null)
            map[x, y - 1].IsDungeonCell = false;

        // West:
        if (Random.value < growChances.west)
            GrowDungeonRecursive(x - 1, y, growSteps);
        else if (ValidCoordinates(x - 1, y) && map[x - 1, y].IsDungeonCell == null)
            map[x - 1, y].IsDungeonCell = false;
    }

    /// <summary>
    /// Mark the cells adjecant to dungeon cells as border.
    /// </summary>
    private void MarkDungeonBorders()
    {
        for (int y = 1; y < map.GetLength(1)-1; y++)
            for (int x = 1; x < map.GetLength(0)-1; x++)
                // Check directions for each dungeon cell and mark non dungeon cells as map borders.
                if (map[x,y].IsDungeonCell == true)
                {
                    if (map[x + 1, y].IsDungeonCell == null)
                        map[x + 1, y].IsDungeonCell = false;
                    if (map[x - 1, y].IsDungeonCell == null)
                        map[x - 1, y].IsDungeonCell = false;
                    if (map[x, y + 1].IsDungeonCell == null)
                        map[x, y + 1].IsDungeonCell = false;
                    if (map[x, y - 1].IsDungeonCell == null)
                        map[x, y - 1].IsDungeonCell = false;
                    if (map[x + 1, y + 1].IsDungeonCell == null)
                        map[x + 1, y + 1].IsDungeonCell = false;
                    if (map[x - 1, y - 1].IsDungeonCell == null)
                        map[x - 1, y - 1].IsDungeonCell = false;
                    if (map[x + 1, y - 1].IsDungeonCell == null)
                        map[x + 1, y - 1].IsDungeonCell = false;
                    if (map[x - 1, y + 1].IsDungeonCell == null)
                        map[x - 1, y + 1].IsDungeonCell = false;
                }
    }

    /// <summary>
    /// Stores its bitmask index in each map tile.
    /// </summary>
    private void CalculateBitmaskValues()
    {
        for (int y = 0; y < map.GetLength(1); y++)
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (map[x,y].IsDungeonCell == true)
                {
                    map[x, y].BitmaskIndex = BitmaskingUtility.GetBitmaskValue4DirectionsDungeonCell(map, x, y);
                    // Building priority for special bitmask indices.
                    if (useSpecialRoomChances)
                    {
                        float rnd = Random.Range(0f, 1f);
                        if ((rnd < special1DRoomChance && map[x, y].BitmaskIndex is 1 or 2 or 4 or 8) || (rnd < special2DSRoomChance && map[x, y].BitmaskIndex is 5 or 10))
                            map[x, y].BuildPriority = CalculateBuildingPriority(map[x, y].BitmaskIndex);
                    }
                }
                else if (spawnMapBorders && map[x,y].IsDungeonCell == false)
                {
                    BitmaskingUtility.Set4DirectionsDungeonBorder(map, x, y);
                }              
            }
    }

    /// <summary>
    /// Instantiates all the prefabs based on the map informations.
    /// </summary>
    private void DrawSimpleDungeon()
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (map[x, y].IsDungeonCell == true)
                {
                    // Check for special room setting.
                    if (map[x, y].BuildPriority != 0)
                    {
                        GameObject tmp = Instantiate(tileAtlas.GetTile(biom, map[x, y].BitmaskIndex), grid.CellToWorld(new Vector3Int(x, 0, y)), transform.rotation, holder);
                        tmp.GetComponent<TileManager>().RandomizeTile(map, x, y, settingsAsset);
                    }
                    else
                    {
                        GameObject tmp = Instantiate(tileAtlas.GetTile(biom), grid.CellToWorld(new Vector3Int(x, 0, y)), transform.rotation, holder);
                        tmp.GetComponent<TileManager>().RandomizeTile(map, x, y, settingsAsset);
                    }
                }
                else if (spawnMapBorders && map[x, y].IsDungeonCell == false)
                {
                    GameObject tmp = Instantiate(tileAtlas.GetBorderTile(biom), grid.CellToWorld(new Vector3Int(x, 0, y)), transform.rotation, holder);
                    tmp.GetComponent<TileManager>().RandomizeMapBorder(map[x,y], settingsAsset);
                }
            }
        }
    }

    /// <summary>
    /// Validates if the given coordinates are within the maps range, excluding the most outer cells of the map, ie the borders.
    /// </summary>
    /// <param name="x">X-coordinate.</param>
    /// <param name="y">Y-coordinate.</param>
    /// <returns>True if the map can have those coordiantes.</returns>
    public bool ValidCoordinates(int x, int y) => x > 0 && x < width-1 && y > 0 && y < height-1;

    /// <summary>
    /// Set the building priority based on specific bitmask indices.
    /// </summary>
    /// <param name="bitmaskIndex">The bitmask index of the cell.</param>
    /// <returns>A build priority to store in the cell.</returns>
    private int CalculateBuildingPriority(int bitmaskIndex)
    {
        if (bitmaskIndex is 1 or 2 or 4 or 8)
            return 1;
        else if (bitmaskIndex is 5 or 10)
            return 2;
        return 0;
    }

    /// <summary>
    /// Basic map initialization.
    /// </summary>
    private void InitMap()
    {
        // Init the map.
        map = new TileInfo[width, height];
        for (int y = 0; y < map.GetLength(1); y++)
            for (int x = 0; x < map.GetLength(0); x++)
                map[x, y] = new TileInfo();
    }
    


    private void OnValidate()
    {
        // Clamp values to reasonable settings.
        width = width < 1 ? 1 : width;
        height = height < 1 ? 1 : height;
    }

    /// <summary>
    /// Growchances for each direction of the dungeon.
    /// </summary>
    [System.Serializable]
    private struct GrowChances
    {
        [Range(0f, 1f)] public float north;
        [Range(0f, 1f)] public float east;
        [Range(0f, 1f)] public float south;
        [Range(0f, 1f)] public float west;
    }
}
