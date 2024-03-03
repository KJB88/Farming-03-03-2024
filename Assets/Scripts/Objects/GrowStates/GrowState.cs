using System.Collections.Generic;

public abstract class GrowState : State, ISubscriber
{
    protected MessageBroker msgBroker;
    protected GrowState(FSM fsm) : base(fsm) { }

    public override void OnStateEnter(Dictionary<string, object> blackboard)
    {
        if (blackboard.TryGetValue("messageBroker", out object obj))
            msgBroker = (MessageBroker)obj;

        msgBroker.RegisterSubscriber("pickedUp", this);
        msgBroker.RegisterSubscriber("unpickedUp", this);
    }

    public override void OnStateExit(Dictionary<string, object> blackboard)
    {
        msgBroker.RemoveSubscriber("pickedUp", this);
        msgBroker.RemoveSubscriber("unpickedUp", this);
    }

    public abstract bool Receive(Message msg);
}
