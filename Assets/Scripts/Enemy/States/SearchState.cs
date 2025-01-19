using UnityEngine;
using UnityEngine.AI;

public class SearchState : BasicState
{
    private float searchTimer;
    private float searchDuration = 5f; 
    private float searchRadius = 10f; 

    protected override void Enter()
    {
        searchTimer = 0;
        enemy.NavMeshAgent.SetDestination(enemy.LastKnownPos);
    }

    protected override void Perform()
    {
        searchTimer += Time.deltaTime;
        if (Vector3.Distance(enemy.transform.position, enemy.LastKnownPos) < 1f)
        {
            Vector3 randomSearchPosition = enemy.LastKnownPos + (Random.insideUnitSphere * searchRadius);
            randomSearchPosition.y = enemy.transform.position.y;
            enemy.NavMeshAgent.SetDestination(randomSearchPosition);
        }

        if (enemy.canSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
        else if (searchTimer > searchDuration)
        {
            stateMachine.ChangeState(new PatrolState());
        }
    }

    protected override void Exit()
    {
        enemy.NavMeshAgent.ResetPath();
    }
}