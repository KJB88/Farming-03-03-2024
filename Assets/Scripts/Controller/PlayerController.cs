using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ISubscriber
{
    [SerializeField] float moveSpeed = 3.0f;

    // Carry
    [SerializeField] Transform localCarryPos;
    [SerializeField] Vector3 currentDropPosition;

    [SerializeField] IPickupable currentCarried;

    List<IPickupable> nearbyCrops = new List<IPickupable>();

    MessageBroker globalMsgBroker;

    bool insideSellZone = false;
    private void Start()
    {
        if (ServiceLocator.RequestService("messageBroker", out IService outService))
            globalMsgBroker = (MessageBroker)outService;

        globalMsgBroker.RegisterSubscriber("destroyObject", this);
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        transform.position += moveSpeed * Time.deltaTime * new Vector3(x, y, 0.0f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Already carrying, dump it
            if (currentCarried != null)
            {
                currentCarried.OnUnpickup(transform.position, insideSellZone);
                currentCarried = null;
                nearbyCrops.Remove(currentCarried);
                return;
            }

            // If no crops nearby, cant pick up!
            if (nearbyCrops.Count == 0)
                return;

            // Pickup from list!
            currentCarried = nearbyCrops[0];
            currentCarried.OnPickup(localCarryPos.transform);
            nearbyCrops.Remove(currentCarried);
        }

        if (currentCarried == null)
            return;

        //currentCarried.UpdatePickup(localCarryPos.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Crop"))
        {
            IPickupable item = collision.GetComponent<IPickupable>();

            if (currentCarried == item)
                return;

            nearbyCrops.Add(item);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("SellBin"))
            insideSellZone = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Crop"))
        {

            IPickupable item = collision.GetComponent<IPickupable>();

            foreach (IPickupable p in nearbyCrops)
            {
                if (p == item)
                {
                    nearbyCrops.Remove(p);
                    break;
                }
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("SellBin"))
            insideSellZone = false;
    }

    public bool Receive(Message msg)
    {
        if (msg.MessageType == "destroyObject")
            currentCarried = null;

        return false;
    }
}
