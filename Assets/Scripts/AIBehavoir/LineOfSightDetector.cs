using UnityEngine;

public class LineOfSightDetector : MonoBehaviour
{
    public float sightRange = 10f;
    public LayerMask obstructionMask;

    private ConvStarter convStarter;
    private bool hasStartedDialogue = false;

    private void Start()
    {
        convStarter = GetComponent<ConvStarter>();
        if (convStarter == null)
        {
            Debug.LogWarning("ConvStarter not found on the NPC!");
        }
    }

    public GameObject PerformDetection(GameObject target)
    {
        if (target == null) return null;

        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget <= sightRange)
        {
            if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
            {
                Debug.DrawRay(transform.position, directionToTarget * distanceToTarget, Color.green);

                if (convStarter != null && !hasStartedDialogue)
                {
                    convStarter.StartDialogue();
                    hasStartedDialogue = true;
                }

                return target;
            }
            else
            {
                Debug.DrawRay(transform.position, directionToTarget * distanceToTarget, Color.red);
            }
        }

        return null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (hasStartedDialogue && other.CompareTag("Player"))
        {
            convStarter.StopDialogue();
            hasStartedDialogue = false;
            Debug.Log("Player exited line of sight - Dialogue stopped.");
        }
    }

    public void ResetDialogue()
    {
        hasStartedDialogue = false;
    }
}
