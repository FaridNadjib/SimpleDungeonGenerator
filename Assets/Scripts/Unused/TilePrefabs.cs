using UnityEngine;

/// <summary>
/// A Scriptable Object storing and returning all the ready to use dungeon tile prefabs.
/// </summary>
//[CreateAssetMenu(fileName = "New Tile Settings", menuName = "SOs/Create new TileSettings")]
public class TilePrefabs : ScriptableObject
{
    // not used anymore.
    [SerializeField] private GameObject[] tilePrefabs;

    public GameObject GetRandomTile() => tilePrefabs[Random.Range(0, tilePrefabs.Length)];
}