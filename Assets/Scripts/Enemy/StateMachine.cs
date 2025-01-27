using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BasicState activeState;
    private Enemy enemy;

    public string CurrentStateName => activeState?.GetType().Name;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        if (enemy == null)
        {
            enabled = false;
            return;
        }

        Initialize();
    }

    private void Update()
    {
        activeState?.PerformState();
    }

    public void Initialize()
    {
        ChangeState(new PatrolState());
    }

    public void ChangeState(BasicState newState)
    {
        if (newState == null) return;

        activeState?.ExitState();
        activeState = newState;
        activeState.Initialize(enemy, this);
        activeState.EnterState();
    }
}