using UnityEngine;

public class AttackState : BasicState
{
    private float moveTimer;
    private float losePlayerTimer;

    protected override void Enter()
    {
    }

    protected override void Exit()
    {
        enemy.NavMeshAgent.ResetPath();
        moveTimer = 0;
        losePlayerTimer = 0;
    }

    protected override void Perform()
    {
        if (enemy.canSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            enemy.NavMeshAgent.SetDestination(enemy.player.transform.position);

            if (moveTimer > Random.Range(3, 7))
            {
                enemy.NavMeshAgent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
            enemy.transform.LookAt(enemy.player.transform);

            enemy.LastKnownPos = enemy.player.transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;

            if (losePlayerTimer > 8)
            {
                stateMachine.ChangeState(new SearchState());
            }
        }
    }
}
