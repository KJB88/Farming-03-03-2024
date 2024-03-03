using UnityEngine;

public interface IPickupable
{
    public void OnPickup();
    public void UpdatePickup(Vector3 newPos);
    public void OnUnpickup(object _); // Provide context as to 'where' its been thrown
}
