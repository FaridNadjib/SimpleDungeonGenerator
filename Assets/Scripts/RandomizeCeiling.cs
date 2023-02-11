using UnityEngine;

/// <summary>
/// Randomizes a ceiling prefab based on settings and pools.
/// </summary>
public class RandomizeCeiling : SpawnHelper, IRandomizable
{
    #region PrefabPools

    [SerializeField, Header("Ceiling prefab:")] private GameObject ceiling;
    [SerializeField, Header("Holder for the spawned prefabs:")] private Transform prefabHolder;

    [SerializeField, Header("Trap Prefabs:")] private SpawnInformation[] trapPrefabs;

    [SerializeField, Header("Decoration Prefabs:")] private SpawnInformation[] decorationPrefabs;

    [SerializeField, Header("Particle Prefabs:")] private SpawnInformation[] particlePrefabs;

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
        SpawnFromPrefabPool(trapPrefabs, settings.GlobalTrapModifier, prefabHolder, ceiling);
        SpawnFromPrefabPool(decorationPrefabs, settings.GlobalDecorationModifier, prefabHolder, ceiling);
        SpawnFromPrefabPool(particlePrefabs, settings.GlobalParticleModifier, prefabHolder, ceiling);

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