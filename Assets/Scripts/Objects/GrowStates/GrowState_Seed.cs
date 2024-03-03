using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrowState_Seed : GrowState, ISubscriber
{
    Transform thisTransform;

    Transform pickerTransform;
    bool carried = false;
    bool justDropped = false;

    TMP_Text stateText;
    public GrowState_Seed(FSM fsm) : base(fsm) { }

    public override void OnStateEnter(Dictionary<string, object> blackboard)
    {
        base.OnStateEnter(blackboard);

        if (blackboard.TryGetValue("thisTransform", out object obj))
            thisTransform = (Transform)obj;

        if (blackboard.TryGetValue("stateText", out obj))
            stateText = (TMP_Text)obj;

        stateText.text = "Plant me!";
    }

    public override void UpdateState(Dictionary<string, object> blackboard)
    {
        if (justDropped)
            fsm.SetState(new GrowState_Planted(fsm), blackboard);

        if (carried)
            thisTransform.position = pickerTransform.position;
    }

    public override void OnStateExit(Dictionary<string, object> blackboard) { base.OnStateExit(blackboard); }

    public override bool Receive(Message msg)
    {
        if (msg.MessageType == "pickedUp")
        {
            MSG_PickedUp pickedUp = (MSG_PickedUp)msg;
            carried = true;
            pickerTransform = pickedUp.picker;
        }
        else if (msg.MessageType == "unpickedUp")
        {
            MSG_UnpickedUp unpickedUp = (MSG_UnpickedUp)msg;
            carried = false;
            thisTransform.position = unpickedUp.placedPos;
            justDropped = true;
        }

        return false;
    }
}
