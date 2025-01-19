using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BasicState activeState;

    void Update()
    {
        if (activeState != null)
        {
            activeState.PerformState();
        }
    }

    public void Initialize()
    {
        ChangeState(new PatrolState());
    }

    public void ChangeState(BasicState newState)
    {
        activeState?.ExitState();

        activeState = newState;

        if (activeState != null)
        {
            activeState.Initialize(GetComponent<Enemy>(), this);
            activeState.EnterState();
        }
    }
}
