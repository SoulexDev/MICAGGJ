using UnityEngine;

public class SuctionTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rb))
        {
            //print(rb);
            rb.AddForce((transform.position - rb.transform.position) * 4 + Vector3.up * 2, ForceMode.Impulse);
        }
    }
}