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

    private GameObject EnemyWeaponHolder;
    [SerializeField] private float eyeHeight;
    private Vector3 lastKnownPosition;
    public Vector3 LastKnownPos { get => lastKnownPosition; set => lastKnownPosition = value; }
    private int currentWaypointIndex = 0;

    [SerializeField] GameObject door;
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
    public bool canSeePlayer()
    {
        if (player == null) return false;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance >= sightDistance) return false;

        Vector3 targetDirection = player.transform.position - transform.position - Vector3.up * eyeHeight;
        float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

        if (angleToPlayer < -fieldOfView || angleToPlayer > fieldOfView) return false;

        Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
        return Physics.Raycast(ray, out RaycastHit hitInfo, sightDistance) && hitInfo.transform.gameObject == player;
    }

    public void OpenDoor()
    {
        Animator doorAnimator = door.GetComponent<Animator>();
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("isOpen", true);
        }
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
            animator.SetBool("isShooting", false);
            EnemyWeaponHolder.gameObject.SetActive(false);
            navMeshAgent.speed = 3.5f;

        }

        if (currentState != "AttackState")
        {
            navMeshAgent.SetDestination(path.waypoints[currentWaypointIndex].position);
            EnemyWeaponHolder.gameObject.SetActive(false);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (bandit.name == "Duck")
            {
                ApplyDamage(collision.gameObject, bandit.damage);
            }
            else if (bandit.name == "Donald")
            {
                ApplyDamage(collision.gameObject, bandit.damage);
            }
        }
    }
    private void ApplyDamage(GameObject target, float damage)
    {
        if (bandit.name == "Duck")
        {
            if (target.CompareTag("Player"))
            {
                PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.playerWaterLevel -= damage; // Damage to player
                    Debug.Log("Player damaged. Current water level: " + playerHealth.playerWaterLevel);
                }
            }
        }
        else if (bandit.name == "Donald")
        {
            if (target.CompareTag("Player"))
            {
                PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.playerMoneyLevel -= damage; // Damage to player
                }
            }
        }
    }
}