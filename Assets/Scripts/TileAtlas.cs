using UnityEngine;

/// <summary>
/// Determines from which prefab pool the map cell will get its tile prefab.
/// </summary>
public enum BiomType
{ Dungeon, Forest, Swamp }

public enum BiomModiferType
{ Dark, Destroyed, Flooded }

/// <summary>
/// This holds and gives access to all the various prefabs of the different biomes.
/// </summary>
[CreateAssetMenu(fileName = "New Tile Atlas", menuName = "SOs/Create new TileAtlas")]
public class TileAtlas : ScriptableObject
{
    #region Prefab Pools

    [SerializeField, Header("Dungeon Biom Tiles:")] private Prefab[] dungeonTiles;
    [SerializeField, Header("Dungeon Biom Border Tiles:")] private Prefab[] dungeonBorderTiles;
    [Space, SerializeField, Header("Forest Biom Tiles:")] private Prefab[] forestTiles;
    [SerializeField, Header("Forest Biom 1 Door Tiles:")] private Prefab[] forest1DTiles;
    [SerializeField, Header("Forest Biom 2 Doors NS Tiles:")] private Prefab[] forest2DNSTiles;
    [SerializeField, Header("Forest Biom 2 Doors EW Tiles:")] private Prefab[] forest2DEWTiles;
    [SerializeField, Header("Forest Biom Border Tiles:")] private Prefab[] forestBorderTiles;

    #endregion Prefab Pools

    /// <summary>
    /// Search the prefab pools for a random object based on biom and bitmask index.
    /// </summary>
    /// <param name="biom">The biom the tile should represent.</param>
    /// <param name="bitmaskIndex">The map cells bitmaskindex.</param>
    /// <returns>A random prefab based on biom and bitmaskindex,</returns>
    public GameObject GetTile(BiomType biom, int bitmaskIndex = 0)
    {
        switch (biom)
        {
            case BiomType.Dungeon:
                return Prefab.GetPrefabByChance(dungeonTiles);

            case BiomType.Forest:
                if (bitmaskIndex == 0)
                    return Prefab.GetPrefabByChance(forestTiles);
                else if (bitmaskIndex is 1 or 2 or 4 or 8)
                    return Prefab.GetPrefabByChance(forest1DTiles);
                else if (bitmaskIndex == 5)
                    return Prefab.GetPrefabByChance(forest2DNSTiles);
                else if (bitmaskIndex == 10)
                    return Prefab.GetPrefabByChance(forest2DEWTiles);
                else
                    return Prefab.GetPrefabByChance(forestTiles);

            case BiomType.Swamp:
                break;

            default:
                break;
        }
        return null;
    }

    /// <summary>
    /// Get a border tile.
    /// </summary>
    /// <param name="biom">Tile based on which biom.</param>
    /// <returns>A map border tile.</returns>
    public GameObject GetBorderTile(BiomType biom)
    {
        switch (biom)
        {
            case BiomType.Dungeon:
                return Prefab.GetPrefabByChance(dungeonBorderTiles);

            case BiomType.Forest:
                return Prefab.GetPrefabByChance(forestBorderTiles);

            case BiomType.Swamp:
                break;

            default:
                break;
        }
        return null;
    }
}