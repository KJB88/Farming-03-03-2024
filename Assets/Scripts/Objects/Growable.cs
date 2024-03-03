using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Growable : MonoBehaviour, IPickupable, ISubscriber
{
    [Header("Data")]
    [SerializeField] public string vegName;
    [SerializeField] public int vegValue;

    [Header("Rendering")]
    [SerializeField] int placedSortOrder = 998;
    [SerializeField] int carriedSortOrder = 1000;

    [Header("Dependencies")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TMP_Text stateText;

    // State management
    FSM fsm;
    Dictionary<string, object> blackboard;
    MessageBroker localMsgBroker;
    MessageBroker globalMsgBroker;

    private void Start()
    {
        localMsgBroker = new MessageBroker();
        localMsgBroker.RegisterSubscriber("destroyObject", this);

        if (ServiceLocator.RequestService("messageBroker", out IService outService))
            globalMsgBroker = (MessageBroker)outService;

        blackboard = new Dictionary<string, object>()
        {
            {"vegName", vegName },
            {"vegValue", vegValue },
            {"stateText", stateText },
            {"messageBroker", localMsgBroker },
            {"globalMessageBroker", globalMsgBroker },
            { "thisTransform", transform }
        };

        fsm = new FSM();
        fsm.SetState(new GrowState_Seed(fsm), blackboard);
    }

    void Update()
    {
        if (fsm == null)
            return;

        fsm.UpdateState(blackboard);
    }

    public void OnPickup(Transform picker)
    {
        spriteRenderer.sortingOrder = 1000;
        localMsgBroker.SendMessage(new MSG_PickedUp("pickedUp", picker)); // Send message with picker transform immediately
    }

    public void OnUnpickup(Vector3 dropPosition, bool insideSellZone = false)
    {
        spriteRenderer.sortingOrder = 998;
        localMsgBroker.SendMessage(new MSG_UnpickedUp("unpickedUp", dropPosition, insideSellZone));
    }

    public bool Receive(Message msg)
    {
        if (msg.MessageType == "destroyObject")
        {
            globalMsgBroker.SendMessage(new Message("destroyObject"));
            Destroy(this.gameObject);
        }

        return true;
    }
}
