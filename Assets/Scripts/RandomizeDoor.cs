using UnityEngine;

/// <summary>
/// Randomizes a door prefab based on settings and pools.
/// </summary>
public class RandomizeDoor : SpawnHelper, IRandomizable
{
    #region PrefabPools

    [SerializeField, Header("Door/Wall prefab:")] private GameObject door;
    [SerializeField, Header("Holder for the spawned prefabs:")] private Transform prefabHolder;

    [SerializeField, Header("Trap Prefabs:")] private SpawnInformation[] trapPrefabs;

    [SerializeField, Header("Decoration Prefabs:")] private SpawnInformation[] decorationPrefabs;

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
        SpawnFromPrefabPool(trapPrefabs, settings.GlobalTrapModifier, prefabHolder, door);
        SpawnFromPrefabPool(decorationPrefabs, settings.GlobalDecorationModifier, prefabHolder, door);

        // Finalize.
        //FinalizeSpawning();
    }

    /// <summary>
    /// Deletes helper spawn transforms from the prefab.
    /// </summary>
    private void FinalizeSpawning()
    {
        if (spawnHelpers != null) DestroyImmediate(spawnHelpers.gameObject);
    }
}