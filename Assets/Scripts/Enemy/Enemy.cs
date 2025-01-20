using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent navMeshAgent;
    [SerializeField] private WaypointPath path;

    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public WaypointPath Path => path;
    public GameObject player;
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    [SerializeField] string currentState;

    [SerializeField] private float eyeHeight;
    private Vector3 lastKnownPosition;
    public Vector3 LastKnownPos { get => lastKnownPosition; set => lastKnownPosition = value; }

    private Animator animator;  // Reference to the Animator component
    private int currentWaypointIndex = 0;  // Index of the current waypoint

    private bool isAttacking = false;  // Flag to track if the enemy is attacking

    private void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();  // Initialize the Animator
        stateMachine.Initialize();
        player = GameObject.FindGameObjectWithTag("Player");

        // Set the agent to move towards the first waypoint
        if (path.waypoints.Count > 0)
        {
            navMeshAgent.SetDestination(path.waypoints[currentWaypointIndex].position);
        }
    }

    private void Update()
    {
        // Update state and check if the enemy can see the player
        canSeePlayer();
        currentState = stateMachine.activeState.ToString();

        // Handle animation and movement
        HandleMovement();

        // Check if the enemy has reached the current waypoint
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
        {
            // Move to the next waypoint in the path
            currentWaypointIndex = (currentWaypointIndex + 1) % path.waypoints.Count;
            navMeshAgent.SetDestination(path.waypoints[currentWaypointIndex].position);
        }
    }

    public bool canSeePlayer()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance < sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position - Vector3.up * eyeHeight;
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform.gameObject == player)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private void HandleMovement()
    {
        if (currentState == "Attack" && !isAttacking)
        {
            // Trigger the Fire animation at the start of Attack state
            animator.SetTrigger("Fire");
            isAttacking = true;  // Mark that the enemy is attacking

            // Stop the movement when attacking
            navMeshAgent.velocity = Vector3.zero;  // Set velocity to zero
            navMeshAgent.isStopped = true;  // Stop the NavMeshAgent from moving
        }
        else if (currentState != "Attack" && isAttacking)
        {
            // Reset attacking state when leaving the Attack state
            isAttacking = false;

            // Allow the agent to move again
            navMeshAgent.isStopped = false;
        }

        // If not attacking, set the movement speed
        if (currentState != "Attack")
        {
            float speed = navMeshAgent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
        }
    }
}
