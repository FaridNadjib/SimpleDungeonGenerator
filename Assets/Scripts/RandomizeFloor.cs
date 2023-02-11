using UnityEngine;

/// <summary>
/// Randomizes a floor prefab based on settings and pools.
/// </summary>
public class RandomizeFloor : SpawnHelper, IRandomizable
{
    #region PrefabPools

    [SerializeField, Header("Floor prefab:")] private GameObject floor;
    [SerializeField, Header("Holder for the spawned prefabs:")] private Transform prefabHolder;

    [SerializeField, Header("Volume Prefabs:")] private Transform volumeOrigin;
    [SerializeField] private Prefab[] volumePrefabs;

    [SerializeField, Header("Fluid Prefabs:")] private SpawnInformation[] fluidPrefabs;

    [SerializeField, Header("Trap Prefabs:")] private SpawnInformation[] trapPrefabs;

    [SerializeField, Header("Decoration Prefabs:")] private SpawnInformation[] decorationPrefabs;

    [SerializeField, Header("Particle Prefabs:")] private SpawnInformation[] particlePrefabs;

    [SerializeField, Header("Enemy Prefabs:")] private SpawnInformation[] enemyPrefabs;

    [SerializeField, Header("Reward Prefabs:")] private SpawnInformation[] rewardPrefabs;

    #endregion PrefabPools

    #region HelperFields

    [SerializeField, Header("Helper Spawn Positions:")] private Transform spawnHelpers;
    [SerializeField, Header("Dungeon Settings:")] private DungeonSettings settings;

    #endregion HelperFields

    /// <summary>
    /// Starts the randomization of the object.
    /// </summary>
    [ContextMenu("RandomizeMe!")]
    public void Randomize()
    {
        // Destroys all previously instantiated dungeon tiles.
        for (int i = prefabHolder.childCount; i > 0; i--)
            DestroyImmediate(prefabHolder.GetChild(0).gameObject);

        // Spawn from the pools.
        SpawnFromPrefabPool(fluidPrefabs, settings.GlobalFluidModifier, prefabHolder, floor);
        SpawnFromPrefabPool(trapPrefabs, settings.GlobalTrapModifier, prefabHolder, floor);
        SpawnFromPrefabPool(rewardPrefabs, settings.GlobalRewardModifier, prefabHolder, floor);
        SpawnFromPrefabPool(decorationPrefabs, settings.GlobalDecorationModifier, prefabHolder, floor);
        SpawnFromPrefabPool(particlePrefabs, settings.GlobalParticleModifier, prefabHolder, floor);
        SpawnFromPrefabPool(enemyPrefabs, settings.GlobalEnemyModifier, prefabHolder, floor);
        // Spawn the volume.
        SpawnVolume();

        // Finalize.
        //FinalizeSpawning();
    }

    /// <summary>
    /// Spawns a volume from a pool.
    /// </summary>
    private void SpawnVolume()
    {
        if (volumePrefabs.Length == 0) return;
        Transform spawnTransform = volumeOrigin != null ? volumeOrigin : transform;
        Instantiate(Prefab.GetPrefabByChance(volumePrefabs), spawnTransform.position, spawnTransform.rotation, prefabHolder);
    }

    /// <summary>
    /// Deletes helper spawn transforms from the prefab.
    /// </summary>
    private void FinalizeSpawning()
    {
        if (spawnHelpers != null) DestroyImmediate(spawnHelpers.gameObject);
    }
}

/// <summary>
/// Calculates a spawn offset based on its settings.
/// </summary>
[System.Serializable]
public class SpawnOffset
{
    [SerializeField] private bool randomSpawnOffset;
    public MinMaxValue<float> xOffset = new MinMaxValue<float>(-5f, 5f);
    public MinMaxValue<float> yOffset = new MinMaxValue<float>(-5f, 5f);
    public MinMaxValue<float> zOffset = new MinMaxValue<float>(-5f, 5f);
    public bool RandomSpawnOffset { get => randomSpawnOffset; set => randomSpawnOffset = value; }

    /// <summary>
    /// Gets an offset vector based on the spawnoffset settings.
    /// </summary>
    /// <param name="offset">The SpawnOffset used to calculate the new offset.</param>
    /// <returns>A spawn offset vector.</returns>
    public Vector3 GetSpawnOffset()
    {
        if (randomSpawnOffset)
        {
            return new Vector3(
                Random.Range(xOffset.Min, xOffset.Max),
                Random.Range(yOffset.Min, yOffset.Max),
                Random.Range(zOffset.Min, zOffset.Max));
        }
        // Return values if not random offset.
        return new Vector3((xOffset.Min + xOffset.Max) * 0.5f, (yOffset.Min + yOffset.Max) * 0.5f, (zOffset.Min + zOffset.Max) * 0.5f);
    }
}

/// <summary>
/// Defines a 2D area.
/// </summary>
[System.Serializable]
public struct SpawnArea
{
    public Transform areaOrigin;
    [Range(0f, 5f), Tooltip("X size (mirrored from negative to its value) of the area.")] public float xRadius;
    [Range(0f, 5f), Tooltip("Y size (mirrored from negative to its value) of the area.")] public float zRadius;

    /// <summary>
    /// Calculates a random point in between the defined bounds.
    /// </summary>
    /// <param name="area">The SpawnArea to use for the calculation.</param>
    /// <returns>A random point in the areas bounds.</returns>
    public static Vector3 GetRandomAreaPoint(SpawnArea area)
        => area.areaOrigin.position + new Vector3(Random.Range(-area.xRadius, area.xRadius), 0f, Random.Range(-area.zRadius, area.zRadius));
}

//// Indexer.
//private T[] content = null;
//public T this[int i] { get { return content[i]; } }