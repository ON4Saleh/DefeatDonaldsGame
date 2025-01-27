using UnityEngine;

public class PatrolState : BasicState
{
    private int currentWaypointIndex;
    private float waitTimer;
    private const float WAIT_TIME = 3f;
    private const float ARRIVAL_DISTANCE = 0.2f;

    protected override void Enter()
    {
        if (!ValidatePatrolPath()) return;
        SetNextWaypoint();
    }

    protected override void Perform()
    {
        if (!ValidatePatrolPath()) return;

        if (enemy.canSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
            return;
        }

        HandlePatrolCycle();
    }

    protected override void Exit()
    {
        enemy.NavMeshAgent.ResetPath();
        waitTimer = 0;
    }

    private bool ValidatePatrolPath()
    {
        if (enemy.Path == null || enemy.Path.waypoints.Count == 0)
        {
            Debug.LogError($"{enemy.name}: No valid patrol path found!");
            return false;
        }
        return true;
    }

    private void HandlePatrolCycle()
    {
        if (!HasReachedWaypoint()) return;

        waitTimer += Time.deltaTime;
        if (waitTimer >= WAIT_TIME)
        {
            SetNextWaypoint();
            waitTimer = 0;
        }
    }

    private bool HasReachedWaypoint()
    {
        return enemy.NavMeshAgent.remainingDistance < ARRIVAL_DISTANCE;
    }
    private void SetNextWaypoint()
    {
        if (enemy.Path == null || enemy.Path.waypoints == null || enemy.Path.waypoints.Count == 0)
        {
            Debug.LogError($"{enemy.name}: Invalid Path or waypoints!");
            return;
        }

        currentWaypointIndex = (currentWaypointIndex + 1) % enemy.Path.waypoints.Count;
        Vector3 nextWaypoint = enemy.Path.waypoints[currentWaypointIndex].position;
        enemy.NavMeshAgent.SetDestination(nextWaypoint);
    }

}