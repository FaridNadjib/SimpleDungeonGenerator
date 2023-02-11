using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper method for the spawning from prefab pools.
/// </summary>
public class SpawnHelper : MonoBehaviour
{
    #region Fields

    private static Ray ray;
    private static RaycastHit hitInfo;
    private static Quaternion tempRotation;
    private static GameObject tempSpawnMask;
    private static SpawnInformation prefab;
    private static List<GameObject> tempLevelGeometry = new List<GameObject>();
    #endregion Fields

    /// <summary>
    /// Spawns objects from the pool in appropiate spots, bases on the spawninformations.
    /// </summary>
    /// <param name="prefabPool">The pool to spawn from.</param>
    /// <param name="modifier">The modifier for the prefab amount.</param>
    /// <param name="prefabHolder">The transform holder object for the spawned prefabs..</param>
    /// <param name="levelGameObject">The base level architecture object.</param>
    protected static void SpawnFromPrefabPool(SpawnInformation[] prefabPool, float modifier, Transform prefabHolder, GameObject levelGameObject)
    {
        if (prefabPool.Length == 0) return;

        for (int i = 0; i < prefabPool.Length; i++)
        {
            prefab = prefabPool[i];
            prefab.InitValues(modifier);

            // Copy settings for prefab only testing. This can be deleted later.
            prefab.tmpSpawnPositions = new List<Transform>(prefab.spawnPositions);

            if (!prefab.UseAreaSpawn)
            {
                // Handle fixed position spawning. No spawn trigger will be checked there atm.
                for (int j = 0; j < prefab.CurrentPrefabGoal; j++)
                {
                    // Still spawn positions left?
                    if (!prefab.HasSpawnPositionsLeft) break;
                    // Spawn prefab.
                    var temp = Instantiate(Prefab.GetPrefabByChance(prefab.prefabs), prefab.GetSpawnPoint(),
                        Quaternion.identity, prefabHolder).transform;
                    tempRotation = prefab.useSpawnPosRotation ? prefab.CurrentTransform.rotation : Quaternion.identity;
                    temp.rotation = tempRotation;
                    // Rotate.
                    if (prefab.useRandomRotation)
                    {
                        temp.Rotate(new Vector3(0, Random.Range(prefab.rotationAmount.Min, prefab.rotationAmount.Max)
                            * (Random.Range(0, prefab.CurrentRotationSteps) + 1), 0), Space.Self);
                    }
                    // Scale.
                    if (prefab.useRandomScale)
                        temp.localScale *= prefab.GetRandomScale();
                }
            }
            else
            {
                // Handle area spawning.
                ray.direction = prefab.spawnArea.areaOrigin.up;
                int currentSpawns = 0;
                tempLevelGeometry.Clear();

                // Create a spawnmask.
                if (prefab.useSpawnMask)
                {
                    tempSpawnMask = Instantiate(prefab.spawnMasks[Random.Range(0, prefab.spawnMasks.Length)].GetSpawnMask(prefab.maskMode), levelGameObject.transform.position, Quaternion.identity);
                    //levelGameObject.layer = 6;
                    if (prefab.maskMode == SpawnMaskMode.Normal)
                        levelGameObject.SetActive(false);
                }

                for (int k = 0; k < prefab.maxSpawnTries; k++)
                {
                    if (currentSpawns >= prefab.CurrentPrefabGoal) break;

                    ray.origin = prefab.GetRandomSpawnAreaPoint();
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        // Spawm prefab.
                        var temp = Instantiate(Prefab.GetPrefabByChance(prefab.prefabs),
                            prefab.GetSpawnPoint(hitInfo.point),
                            Quaternion.identity, prefabHolder).transform;

                        // Rotate.
                        if (prefab.rotationIsHitNormal)
                            temp.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                        if (prefab.useRandomRotation)
                            temp.Rotate(new Vector3(0, Random.Range(prefab.rotationAmount.Min, prefab.rotationAmount.Max), 0)
                                * (Random.Range(0, prefab.CurrentRotationSteps) + 1), Space.Self);
                        // Scale.
                        if (prefab.useRandomScale)
                            temp.localScale *= prefab.GetRandomScale();

                        // // Check successful spawn. Do it here cause final pos and scale first.
                        if (temp.TryGetComponent(out ICheckSpawnable check))
                            if (check.CheckCollision())
                            {
                                if (prefab.useDotAngleLimits)
                                {
                                    if (Vector3.Dot(temp.up, Vector3.up) >= prefab.dotAngleLimit.Min
                                        && Vector3.Dot(temp.up, Vector3.up) <= prefab.dotAngleLimit.Max)
                                        currentSpawns++;
                                    else
                                        DestroyImmediate(temp.gameObject);
                                }
                                if (!prefab.useDotAngleLimits) currentSpawns++;
                                if(temp != null) tempLevelGeometry.Add(temp.gameObject);
                            }
                            else
                                DestroyImmediate(temp.gameObject);
                    }
                }
                if (prefab.addToBaseLevelGeometry)
                    for (int m = 0; m < tempLevelGeometry.Count; m++)
                        tempLevelGeometry[m].layer = 6;
                // Destroy the spawnmask.
                if (prefab.useSpawnMask)
                {
                    DestroyImmediate(tempSpawnMask);
                    if (prefab.maskMode == SpawnMaskMode.Normal)
                        levelGameObject.SetActive(true);
                }
            }
        }
    }
}