using UnityEngine;

public class EasterAOE : MonoBehaviour
{
    [SerializeField] private Posessor m_Posessor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rb))
        {
            //print(rb);
            rb.AddForce((transform.position - rb.transform.position) * 4 + Vector3.up * 2, ForceMode.Impulse);
        }
        if (Random.value > 0.8f)
        {
            if (other.TryGetComponent(out Posessable p))
            {
                Instantiate(m_Posessor, transform.position, Quaternion.identity).Initialize(p);
            }
        }
        if (other.TryGetComponent(out FlickerLight flickerLight))
        {
            flickerLight.StartFlicker();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out FlickerLight flickerLight))
        {
            flickerLight.EndFlicker();
        }
    }
}