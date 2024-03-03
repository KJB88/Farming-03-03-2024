using System.Collections.Generic;

public abstract class State
{
    protected FSM fsm;

    public State(FSM fsm)
       => this.fsm = fsm;

    public abstract void OnStateEnter(Dictionary<string, object> blackboard);
    public abstract void OnStateExit(Dictionary<string, object> blackboard);
    public abstract void UpdateState(Dictionary<string, object> blackboard);
}
