
/// <summary>
/// Interface for objects with collision trigger check when spawned.
/// </summary>
public interface ICheckSpawnable
{
    /// <summary>
    /// Checks if a collision occured.
    /// </summary>
    /// <returns>True if no collision occured, ie valid spawn position.</returns>
    public bool CheckCollision();
}
