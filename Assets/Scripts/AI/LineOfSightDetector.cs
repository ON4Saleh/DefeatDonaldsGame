using UnityEngine;

public class LineOfSightDetector : MonoBehaviour
{
    public float sightRange = 10f; // Maximum detection distance
    public LayerMask obstructionMask; // Layers that block vision

    public GameObject PerformDetection(GameObject target)
    {
        if (target == null) return null;

        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        // Perform a raycast to check if there's a clear line of sight
        if (distanceToTarget <= sightRange)
        {
            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
            {
                Debug.DrawRay(transform.position, directionToTarget * distanceToTarget, Color.green); // Debugging ray
                return target; // Target is visible
            }
            else
            {
                Debug.DrawRay(transform.position, directionToTarget * distanceToTarget, Color.red); // Debugging ray
            }
        }

        return null; // No line of sight
    }
}
