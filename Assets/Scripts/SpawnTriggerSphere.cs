using UnityEngine;

/// <summary>
/// Checks for collisions in sphere shape.
/// </summary>
public class SpawnTriggerSphere : MonoBehaviour, ICheckSpawnable
{
    #region Fields
    [SerializeField] bool showCollisionGizmo = true;
    [SerializeField] float checkOffset;
    [SerializeField] float checkRadius;
    [SerializeField] LayerMask mask; 
    #endregion

    /// <summary>
    /// Checks if a collision occured. If not, ie vaild position, enable its collider and remove this script.
    /// </summary>
    /// <returns>True if no collision occured, ie valid spawn position.</returns>
    public bool CheckCollision()
    {
        if (Physics.CheckSphere(transform.position + checkOffset * transform.localScale.x * transform.up, checkRadius * transform.localScale.x, mask))
        {
            return false;
        }
        else
        {
            // Valid position, so activate the objects collider and remove this script.
            if (TryGetComponent(out Collider col))
                col.enabled = true;
            DestroyImmediate(GetComponent<SpawnTriggerSphere>());
            return true;
        }
    }

    private void OnDrawGizmos()
    {
        if(showCollisionGizmo)
            Gizmos.DrawWireSphere(transform.position + checkOffset * transform.localScale.x * transform.up, checkRadius * transform.localScale.x);
    }
}
