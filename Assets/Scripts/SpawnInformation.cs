using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class SpawnInformation
{
    [SerializeField ,Header("Spawn options:")] private bool useAreaSpawn;
    [ShowIf("useAreaSpawn")] public int maxSpawnTries = 20;
    [SerializeField] bool ignoreGlobalSettings;
    public MinMaxValue<int> prefabAmount = new MinMaxValue<int>(0, 50, 2, 10);
    private int currentPrefabGoal;

    [ShowIf("useAreaSpawn")] public bool rotationIsHitNormal;
    [ShowIf("useAreaSpawn")] public bool useSpawnMask;
    [ShowIf("useSpawnMask")] public SpawnMaskMode maskMode;
    [ShowIf("useSpawnMask")] public SpawnMask[] spawnMasks;
    [ShowIf("useAreaSpawn")] public SpawnArea spawnArea;
    [ShowIf("useAreaSpawn")] public bool addToBaseLevelGeometry;
    [ShowIf("useAreaSpawn")] public bool useDotAngleLimits;
    [ShowIf("useAreaSpawn"),ShowIf("useDotAngleLimits")] public MinMaxValue<float> dotAngleLimit = new MinMaxValue<float>(-1f, 1f);

    public bool useSpawnOffset;
    [ShowIf("useSpawnOffset")] public SpawnOffset spawnOffset;
    [HideIf("useAreaSpawn")] public List<Transform> spawnPositions;
    [HideIf("useAreaSpawn")] public bool useSpawnPosRotation;
    public List<Transform> tmpSpawnPositions { get; set; }
    public bool HasSpawnPositionsLeft => tmpSpawnPositions.Count != 0;

    [Header("Scale options:")] public bool useRandomScale;
    [ShowIf("useRandomScale")] public MinMaxValue<float> scaleMultiplier = new MinMaxValue<float>(0.1f, 3f, 0.9f, 1.1f);

    [Header("Rotation options:")] public bool useRandomRotation;
    [ShowIf("useRandomRotation")] public bool useRotationSteps;
    private int currentRotationSteps;
    [ShowIf("useRandomRotation")] public MinMaxValue<float> rotationAmount = new MinMaxValue<float>(-180f, 180f);

    [Header("Prefab pool:")] public Prefab[] prefabs;

    public Transform CurrentTransform { get; set; }
    public int CurrentPrefabGoal { get => currentPrefabGoal; set => currentPrefabGoal = value; }
    public int CurrentRotationSteps { get => currentRotationSteps; set => currentRotationSteps = value; }
    public bool UseAreaSpawn { get => useAreaSpawn; set => useAreaSpawn = value; }

    public Vector3 GetSpawnPoint()
    {
        this.CurrentTransform = this.GetRandomSpawnTransform();
        if (this.useSpawnOffset)
            return this.CurrentTransform.position + spawnOffset.GetSpawnOffset();
        else
            return this.CurrentTransform.position;
    }
    public Vector3 GetSpawnPoint(Vector3 hitPoint)
    {
        if (useSpawnOffset)
            return hitPoint + spawnOffset.GetSpawnOffset();
        else
            return hitPoint;
    }

    private Transform GetRandomSpawnTransform()
    {
        int index = Random.Range(0, this.tmpSpawnPositions.Count);
        Transform tmp = this.tmpSpawnPositions[index];
        this.tmpSpawnPositions.RemoveAt(index);
        return tmp;
    }

    public Vector3 GetRandomSpawnAreaPoint()
        => spawnArea.areaOrigin.position + new Vector3(Random.Range(-spawnArea.xRadius, spawnArea.xRadius), 0f, Random.Range(-spawnArea.zRadius, spawnArea.zRadius));

    public float GetRandomScale() => Random.Range(scaleMultiplier.Min, scaleMultiplier.Max);

    public void InitValues(float modifier)
    {
        if (useRotationSteps)
            CurrentRotationSteps = 360 / (int)((rotationAmount.Min + rotationAmount.Max) * 0.5f);
        else
            CurrentRotationSteps = 0;

        CurrentPrefabGoal = ignoreGlobalSettings ? Random.Range(prefabAmount.Min, prefabAmount.Max + 1) 
            : (int)(Random.Range(prefabAmount.Min, prefabAmount.Max + 1) * modifier);
    }
}


