using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class stores informations about a single cell the dungeon map is made of.
/// </summary>
public class TileInfo
{
    #region Properties
    /// <summary>
    /// True = map coordinate is dungeon cell, false = not, null = hasnt been visited at all.
    /// </summary>
    public bool? IsDungeonCell { get; set; }
    public bool IsStartingRoom { get; set; }
    public bool HasCeiling { get; set; }
    public int BitmaskIndex { get; set; }
    public int BuildPriority { get; set; }
    public BiomType Biom { get; set; }
    public BiomModiferType BiomModifier { get; set; }
    /// <summary>
    /// A 4 length array each entry representing a door direction, starting with north going clockwise.
    /// </summary>
    public bool[] DoorsSpawned { get; set; } 
    /// <summary>
    /// A 4 length array each entry set to true represents a mapborder direction, starting with north going clockwise.
    /// </summary>
    public bool[] SpawnMapBorder { get; set; }
    #endregion

    #region Constructors
    public TileInfo()
    {
        DoorsSpawned = new bool[4];
        SpawnMapBorder = new bool[4];
    }
    #endregion
}
