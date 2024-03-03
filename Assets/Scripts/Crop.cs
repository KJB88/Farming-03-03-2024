using System;
using UnityEngine;

public class Crop : MonoBehaviour, IPickupable
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Vector3 carryOffset; // Crop specific carry offset


    public void OnPickup()
    {
        spriteRenderer.sortingOrder = 1000;
    }

    public void UpdatePickup(Vector3 newPos)
        => transform.position = newPos + carryOffset;

    public void OnUnpickup(object _)
    {
        spriteRenderer.sortingOrder = 998;
        Destroy(this.gameObject);
    }
}
