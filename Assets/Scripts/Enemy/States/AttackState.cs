using UnityEngine;

public class AttackState : BasicState
{
    private float moveTimer;
    private float losePlayerTimer;
    private const float MAX_LOSE_TIME = 8f;
    private const float MIN_MOVE_TIME = 3f;
    private const float MAX_MOVE_TIME = 7f;
    private const float MOVE_RADIUS = 5f;

    protected override void Enter()
    {
        ResetTimers();
    }

    protected override void Perform()
    {
        if (enemy.canSeePlayer())
        {
            HandleVisiblePlayer();
        }
        else
        {
            HandleLostPlayer();
        }
    }

    protected override void Exit()
    {
        enemy.NavMeshAgent.ResetPath();
        ResetTimers();
    }

    private void ResetTimers()
    {
        moveTimer = 0;
        losePlayerTimer = 0;
    }

    private void HandleVisiblePlayer()
    {
        losePlayerTimer = 0;
        moveTimer += Time.deltaTime;

        UpdateLastKnownPosition();
        RotateTowardsPlayer();

        if (ShouldRepositionEnemy())
        {
            RepositionEnemy();
        }
        else
        {
            ChasePlayer();
        }
    }

    private void HandleLostPlayer()
    {
        losePlayerTimer += Time.deltaTime;
        if (losePlayerTimer > MAX_LOSE_TIME)
        {
            stateMachine.ChangeState(new SearchState());
        }
    }

    private void UpdateLastKnownPosition()
    {
        if (enemy.player != null)
        {
            enemy.LastKnownPos = enemy.player.transform.position;
        }
    }

    private void RotateTowardsPlayer()
    {
        if (enemy.player != null)
        {
            enemy.transform.LookAt(enemy.player.transform);
        }
    }

    private bool ShouldRepositionEnemy()
    {
        return moveTimer > Random.Range(MIN_MOVE_TIME, MAX_MOVE_TIME);
    }

    private void RepositionEnemy()
    {
        Vector3 randomPosition = enemy.transform.position + (Random.insideUnitSphere * MOVE_RADIUS);
        randomPosition.y = enemy.transform.position.y;
        enemy.NavMeshAgent.SetDestination(randomPosition);
        moveTimer = 0;
    }

    private void ChasePlayer()
    {
        if (enemy.player != null)
        {
            enemy.NavMeshAgent.SetDestination(enemy.player.transform.position);
        }
    }
}