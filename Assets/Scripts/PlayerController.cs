using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3.0f;

    // Carry
    [SerializeField] Transform localCarryPos;
    [SerializeField] Vector3 currentDropPosition;

    [SerializeField] IPickupable currentCarried;

    List<IPickupable> nearbyCrops = new List<IPickupable>();

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
                currentCarried.OnUnpickup(this);
                currentCarried = null;
                return;
            }

            // If no crops nearby, cant pick up!
            if (nearbyCrops.Count == 0)
                return;

            // Pickup from list!
            currentCarried = nearbyCrops[0];
            currentCarried.OnPickup();
            nearbyCrops.Remove(currentCarried);
        }

        if (currentCarried == null)
            return;

        currentCarried.UpdatePickup(localCarryPos.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Crop"))
            return;

        IPickupable item = collision.GetComponent<IPickupable>();

        if (currentCarried == item)
            return;

        nearbyCrops.Add(item);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Crop"))
            return;

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
}
