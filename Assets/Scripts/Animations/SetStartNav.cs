using UnityEngine;
using UnityEngine.AI;

public class SetStartPoint : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Vector3 newStartPosition = new Vector3(5f, 0f, 5f);  
        transform.position = newStartPosition;
        agent.SetDestination(new Vector3(10f, 0f, 10f)); 
    }
}
