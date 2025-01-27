using UnityEngine;

public class SearchState : BasicState
{
    private float searchTimer;
    private const float SEARCH_DURATION = 5f;
    private const float SEARCH_RADIUS = 10f;
    private const float ARRIVAL_DISTANCE = 1f;

    protected override void Enter()
    {
        searchTimer = 0;
        enemy.NavMeshAgent.SetDestination(enemy.LastKnownPos);
    }

    protected override void Perform()
    {
        searchTimer += Time.deltaTime;

        if (enemy.canSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
            return;
        }

        if (searchTimer > SEARCH_DURATION)
        {
            stateMachine.ChangeState(new PatrolState());
            return;
        }

        if (HasReachedLastKnownPosition())
        {
            SearchRandomNearbyPosition();
        }
    }

    protected override void Exit()
    {
        enemy.NavMeshAgent.ResetPath();
    }

    private bool HasReachedLastKnownPosition()
    {
        return Vector3.Distance(enemy.transform.position, enemy.LastKnownPos) < ARRIVAL_DISTANCE;
    }

    private void SearchRandomNearbyPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * SEARCH_RADIUS;
        randomDirection.y = 0;
        Vector3 searchPosition = enemy.LastKnownPos + randomDirection;
        enemy.NavMeshAgent.SetDestination(searchPosition);
    }
}