using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GrowState_Planted : GrowState, ISubscriber
{
    float growthCounter = 0;
    float maxGrowth = 10.0f;

    TMP_Text stateText;
    public GrowState_Planted(FSM fsm) : base(fsm) { }

    public override void OnStateEnter(Dictionary<string, object> blackboard)
    {
        base.OnStateEnter(blackboard);

        if (blackboard.TryGetValue("stateText", out object obj))
            stateText = (TMP_Text)obj;
    }

    public override void UpdateState(Dictionary<string, object> blackboard)
    {
        growthCounter += Time.deltaTime;
        stateText.text = "Growing... \n" + growthCounter / maxGrowth * 100.0f + "%";

        if (growthCounter >= maxGrowth)
            fsm.SetState(new GrowState_Mature(fsm), blackboard);
    }

    public override void OnStateExit(Dictionary<string, object> blackboard) { base.OnStateExit(blackboard); }

    public override bool Receive(Message msg)
    {
        if (msg.MessageType == "pickedUp")
        {
            MSG_PickedUp pickedUp = (MSG_PickedUp)msg;
            msgBroker.SendMessage(new Message("destroyObject"));
        }

        return false;
    }
}
