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
    public WaypointPath WayPath => path;
    public GameObject player;
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    [SerializeField] string currentState;
    private Transform playerTransform; 
    private GameObject EnemyWeaponHolder;
    [SerializeField] private float eyeHeight;
    private Vector3 lastKnownPosition;
    public Vector3 LastKnownPos { get => lastKnownPosition; set => lastKnownPosition = value; }
    private int currentWaypointIndex = 0;
    public Door door;
 

    private Bandits bandit;
    private PlayerHealth playerHealth;
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
        EnemyWeaponHolder = transform.GetComponentInChildren<Transform>().Find("EnemyWeaponHolder")?.gameObject;
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
    public void Die()
    {
        Debug.Log("Enemy died!"); 
        door.OpenDoor(); 
        Destroy(gameObject);
    }
      public void StopShootingAnimation()
    {
        animator.SetBool("isShooting", false);
    }
    public bool canSeePlayer()
    {
        if (player != null)
        {
            playerTransform = player.transform; 
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
                        while (hitInfo.transform.gameObject == player)
                        {
                            Debug.Log("enemy can see player");
                          
                            return true;
                        }
                    }
                }
            }
        }
        Debug.Log("enemy cannot see player");
        StopShootingAnimation();
        return false;
    }


    private void HandleMovement()
    {
        if (currentState == "AttackState" && !animator.GetBool("isShooting"))
        {
            if (canSeePlayer())
            {
                animator.SetBool("isShooting", true);
                navMeshAgent.speed = 1.5f;
                Weapon.HandleShooting();
                EnemyWeaponHolder.gameObject.SetActive(true);

                SoundManager.Instance.PlaySFX("Trump");
            }
            StartCoroutine(Weapon.EnemyBurstFire());
        }
        else if (currentState != "AttackState" && animator.GetBool("isShooting"))
        {
            //animator.SetBool("isShooting", false);
            EnemyWeaponHolder.gameObject.SetActive(false);
            navMeshAgent.speed = 3.5f;

        }

        if (currentState != "AttackState")
        {
            navMeshAgent.SetDestination(path.waypoints[currentWaypointIndex].position);
            EnemyWeaponHolder.gameObject.SetActive(false);

        }
    }

}