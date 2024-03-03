using System.Collections.Generic;

public class FSM
{
    State currentState;

    public FSM() { }

    public void SetState(State state, Dictionary<string, object> blackboard)
    {
        if (currentState != null)
            currentState.OnStateExit(blackboard);
        currentState = state;
        currentState.OnStateEnter(blackboard);
    }

    public void UpdateState(Dictionary<string, object> blackboard)
    {
        if (currentState != null)
            currentState.UpdateState(blackboard);
    }
}