using UnityEngine;

public abstract class BasicState
{
    protected Enemy enemy;
    protected StateMachine stateMachine;

    protected abstract void Enter();
    protected abstract void Perform();
    protected abstract void Exit();

    public void Initialize(Enemy enemyInstance, StateMachine stateMachineInstance)
    {
        this.enemy = enemyInstance;
        this.stateMachine = stateMachineInstance;
    }

    public void EnterState() => Enter();
    public void PerformState() => Perform();
    public void ExitState() => Exit();
}