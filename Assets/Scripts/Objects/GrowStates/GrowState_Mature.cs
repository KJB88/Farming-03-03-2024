using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class GrowState_Mature : GrowState, ISubscriber
{
    Transform pickerTransform;
    Transform thisTransform;

    TMP_Text stateText;
    MessageBroker globalMsgBroker;

    bool carried = false;
    bool justDropped = false;

    public GrowState_Mature(FSM fsm) : base(fsm) { }

    public override void OnStateEnter(Dictionary<string, object> blackboard)
    {
        base.OnStateEnter(blackboard);

        if (blackboard.TryGetValue("thisTransform", out object obj))
            thisTransform = (Transform)obj;

        if (blackboard.TryGetValue("stateText", out obj))
            stateText = (TMP_Text)obj;

        if (blackboard.TryGetValue("globalMessageBroker", out obj))
            globalMsgBroker = (MessageBroker)obj;

        stateText.text = "Grown! \n Pick me up & sell me!";
    }

    public override void UpdateState(Dictionary<string, object> blackboard)
    {
        if (justDropped)
        {
            msgBroker.SendMessage(new Message("destroyObject"));
            justDropped = false;
        }

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
            if (unpickedUp.inDropZone)
                globalMsgBroker.SendMessage(new Message("cropSold"));

            carried = false;
            justDropped = true;
        }

        return false;
    }
}
