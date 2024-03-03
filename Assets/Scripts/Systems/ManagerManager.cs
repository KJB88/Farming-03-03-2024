using UnityEngine;

public class ManagerManager : MonoBehaviour
{
    private void Awake()
    {
        MessageBroker messageBroker = new MessageBroker();
        ServiceLocator.AddService("messageBroker", messageBroker);
    }
}
