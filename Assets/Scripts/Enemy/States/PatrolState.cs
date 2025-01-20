using UnityEngine;

public class PatrolState : BasicState
{
    private int waypointIndex;
    private float waitTimer;

    protected override void Enter()
    {
        if (enemy.Path.waypoints.Count > 0)
        {
            waypointIndex = 0;
            enemy.NavMeshAgent.SetDestination(enemy.Path.waypoints[waypointIndex].position);
            Debug.Log("Patrol started, destination: " + enemy.Path.waypoints[waypointIndex].position);
        }
        else
        {
            Debug.LogError("No waypoints in the path!");
        }
    }

    protected override void Perform()
    {
        PatrolCycle();
        Debug.Log("Current destination: " + enemy.NavMeshAgent.destination);
        if (enemy.canSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    protected override void Exit()
    {
        enemy.NavMeshAgent.ResetPath();
        waitTimer = 0;
    }

    private void PatrolCycle()
    {
        waitTimer += Time.deltaTime;
        if (enemy.NavMeshAgent.remainingDistance < 0.2f && enemy.NavMeshAgent.remainingDistance >= 0f)
        {
            if (waitTimer > 3)
            {
                waypointIndex = (waypointIndex + 1) % enemy.Path.waypoints.Count;
                enemy.NavMeshAgent.SetDestination(enemy.Path.waypoints[waypointIndex].position);
                waitTimer = 0;
            }
        }
    }
}
