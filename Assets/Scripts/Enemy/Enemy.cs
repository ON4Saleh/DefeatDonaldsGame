//using UnityEngine;
//using UnityEngine.AI;

//public class Enemy : MonoBehaviour
//{
//    private StateMachine stateMachine;
//    private NavMeshAgent navMeshAgent;
//    [SerializeField] private WaypointPath path;

//    public NavMeshAgent NavMeshAgent => navMeshAgent;
//    public WaypointPath Path => path;
//    public GameObject player;
//    public float sightDistance = 20f;
//    public float fieldOfView = 85f;
//    [SerializeField] string currentState;

//    [SerializeField] private float eyeHeight;
//    private Vector3 lastKnownPosition;
//    public Vector3 LastKnownPos { get => lastKnownPosition; set => lastKnownPosition = value; }

//    private void Start()
//    {
//        stateMachine = GetComponent<StateMachine>();
//        navMeshAgent = GetComponent<NavMeshAgent>();
//        stateMachine.Initialize();
//        player = GameObject.FindGameObjectWithTag("Player");
//    }

//    private void Update()
//    {
//        canSeePlayer();
//        currentState = stateMachine.activeState.ToString();
//    }
//    public bool canSeePlayer()
//    {
//        if (player != null)
//        {
//            float distance = Vector3.Distance(transform.position, player.transform.position);

//            if (distance < sightDistance)
//            {
//                Vector3 targetDirection = player.transform.position - transform.position - Vector3.up * eyeHeight;
//                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

//                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
//                {
//                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
//                    RaycastHit hitInfo;

//                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
//                    {
//                        if (hitInfo.transform.gameObject == player)
//                        {
//                            return true;
//                        }
//                    }
//                }
//            }
//        }
//        return false;
//    }
//}
