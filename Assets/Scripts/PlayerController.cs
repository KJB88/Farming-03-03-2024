using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3.0f;

    // Carry
    [SerializeField] Transform localCarryPos;
    [SerializeField] IPickupable currentCarried;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        transform.position += moveSpeed * Time.deltaTime * new Vector3(x, y, 0.0f);

        if (currentCarried != null)
            currentCarried.UpdatePickup(localCarryPos.position);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Crop"))
        {
            currentCarried = collision.GetComponent<IPickupable>();
            currentCarried.OnPickup();
        }
    }
}
