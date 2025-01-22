using UnityEngine.AI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent navMeshAgent;
    [SerializeField] private WaypointPath path;
    private Animator animator;
    private Weapon Weapon;
    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public WaypointPath Path => path;
    public GameObject player;
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    [SerializeField] string currentState;

    private GameObject EnemyWeaponHolder;
    [SerializeField] private float eyeHeight;
    private Vector3 lastKnownPosition;
    public Vector3 LastKnownPos { get => lastKnownPosition; set => lastKnownPosition = value; }
    private bool trumpPlayed = false;
    private int currentWaypointIndex = 0;

    private void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Weapon = GetComponent<Weapon>();
        stateMachine.Initialize();
        player = GameObject.FindGameObjectWithTag("Player");

        if (path.waypoints.Count > 0)
        {
            navMeshAgent.SetDestination(path.waypoints[currentWaypointIndex].position);
        }
        EnemyWeaponHolder = transform.Find("EnemyWeaponHolder")?.gameObject;
        Weapon = EnemyWeaponHolder.GetComponentInChildren<Weapon>();
        
    }

    private void Update()
    {
        currentState = stateMachine.activeState.ToString();
        if (currentState != "AttackState")
        {
            canSeePlayer();
        }

        HandleMovement();
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % path.waypoints.Count;
            navMeshAgent.SetDestination(path.waypoints[currentWaypointIndex].position);
        }
    }

    public bool canSeePlayer()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            // ???? ??? ???? ??????? ??? ???? ??????
            if (distance < sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position - Vector3.up * eyeHeight;
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

                // ???? ?? ?? ?????? ?? ????? ??????
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo;

                    // ???? ?? ?? ?????? ????? ???????
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
        if (currentState == "AttackState" && !animator.GetBool("isShooting"))
        {
            if (canSeePlayer())
            {
                animator.SetBool("isShooting", true);
                Debug.Log("Setting isShooting to true");
                navMeshAgent.speed = 1.5f;
                Weapon.HandleShooting();
                EnemyWeaponHolder.gameObject.SetActive(true);
                trumpPlayed = true;
                SoundManager.Instance.PlaySFX("Trump");  
            }
        }
        else if (currentState != "AttackState" && animator.GetBool("isShooting"))
        {
            animator.SetBool("isShooting", false);
            EnemyWeaponHolder.gameObject.SetActive(false);
            navMeshAgent.speed = 3.5f; 
            trumpPlayed = false;
        }

        if (currentState != "AttackState")
        {
            navMeshAgent.SetDestination(path.waypoints[currentWaypointIndex].position);
            EnemyWeaponHolder.gameObject.SetActive(false);
            trumpPlayed = false;
        }
    }

}
