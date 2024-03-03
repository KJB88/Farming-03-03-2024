using UnityEngine;

public interface IPickupable
{
    public void OnPickup(Transform picker);
    public void OnUnpickup(Vector3 dropPos, bool insideSellZone = false); // Provide context as to 'where' its been thrown
}
