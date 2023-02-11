using UnityEngine;

/// <summary>
/// Calculates the bitmask value of a given cell.
/// </summary>
public static class BitmaskingUtility
{
    /// <summary>
    /// Calculates the bitmask value of a given cell.
    /// </summary>
    /// <param name="array">The 2D array the cell is part of.</param>
    /// <param name="x">Its x coordinate.</param>
    /// <param name="y">Its x coordinate.</param>
    /// <returns>The bitmask vale.</returns>
    public static int GetBitmaskValue4DirectionsDungeonCell(TileInfo[,] array, int x, int y)
    {
        // Save array dimensions.
        int width = array.GetLength(0);
        int height = array.GetLength(1);

        // Check for valid coordinates. 0 bitmask value for default invalid coordinates.
        if (x < 0 || x >= width || y < 0 || y >= height)
            return 0;

        // Check for all 4 directions, if the adjectant tile is same as the coordinate tile.
        int result = 0;
        int powCounter = 0;

        // Check north:
        if (y + 1 < height)
            result += array[x, y + 1].IsDungeonCell == true ? (int)Mathf.Pow(2, powCounter) : 0;
        powCounter++;

        // Check east:
        if (x + 1 < width)
            result += array[x + 1, y].IsDungeonCell == true ? (int)Mathf.Pow(2, powCounter) : 0;
        powCounter++;

        // Check south:
        if (y - 1 >= 0)
            result += array[x, y - 1].IsDungeonCell == true ? (int)Mathf.Pow(2, powCounter) : 0;
        powCounter++;

        // Check west:
        if (x - 1 >= 0)
            result += array[x - 1, y].IsDungeonCell == true ? (int)Mathf.Pow(2, powCounter) : 0;

        return result;
    }

    /// <summary>
    /// Sets the map border parameter of the given tile.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public static void Set4DirectionsDungeonBorder(TileInfo[,] array, int x, int y)
    {
        // Save array dimensions.
        int width = array.GetLength(0);
        int height = array.GetLength(1);

        // Check for valid coordinates. 0 bitmask value for default invalid coordinates.
        if (x < 0 || x >= width || y < 0 || y >= height)
            return;

        // Check for all 4 directions, if the adjectant tile is null, meaning border.
        // Check north:
        if (y + 1 < height)
            array[x, y].SpawnMapBorder[0] = array[x, y + 1].IsDungeonCell == null ? true : false;
        else if(y+1 == height)
            array[x, y].SpawnMapBorder[0] = true;

        // Check east:
        if (x + 1 < width)
            array[x, y].SpawnMapBorder[1] = array[x + 1, y].IsDungeonCell == null ? true : false;
        else if (x + 1 == width)
            array[x, y].SpawnMapBorder[1] = true;

        // Check south:
        if (y - 1 > 0)
            array[x, y].SpawnMapBorder[2] = array[x, y - 1].IsDungeonCell == null ? true : false;
        else if (y - 1 == 0)
            array[x, y].SpawnMapBorder[2] = true;

        // Check west:
        if (x - 1 >= 0)
            array[x, y].SpawnMapBorder[3] = array[x - 1, y].IsDungeonCell == null ? true : false;
        else if (x - 1 == -1)
            array[x, y].SpawnMapBorder[3] = true;
    }
}