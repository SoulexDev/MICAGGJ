using UnityEngine;

public class RigidbodySound : MonoBehaviour
{
    public AdvancedAudioSource advancedAudioSource;
    public float soundThreshold = 0.5f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > soundThreshold)
        {
            advancedAudioSource.PlayOneShot();
        }
    }
}