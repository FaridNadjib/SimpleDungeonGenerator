using UnityEngine;

/// <summary>
/// The working mode for spawn masks.
/// </summary>
public enum SpawnMaskMode
{ Normal, Inverted, Add }

/// <summary>
/// Creates a new spawn mask object.
/// </summary>
[CreateAssetMenu(fileName = "New Spawn Mask", menuName = "SOs/Create new SpawnMask")]
public class SpawnMask : ScriptableObject
{
    [SerializeField, Header("The mask object:")] private GameObject maskObject;
    public GameObject MaskObject { get => maskObject; set => maskObject = value; }

    /// <summary>
    /// Returns the modified mask object.
    /// </summary>
    /// <param name="maskMode">The mode the mask should work by.</param>
    /// <returns>Returns the modified mask object.</returns>
    public GameObject GetSpawnMask(SpawnMaskMode maskMode)
    {
        switch (maskMode)
        {
            case SpawnMaskMode.Normal:
            case SpawnMaskMode.Add:
                foreach (Transform child in MaskObject.transform)
                {
                    child.gameObject.layer = 6;
                }
                break;

            case SpawnMaskMode.Inverted:
                foreach (Transform child in MaskObject.transform)
                {
                    child.gameObject.layer = 0;
                }
                break;

            default:
                break;
        }

        return MaskObject;
    }
}