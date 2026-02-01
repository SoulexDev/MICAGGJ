using UnityEngine;

public class DoorSounds : MonoBehaviour
{
    [SerializeField] private AdvancedAudioSource m_AudioSource;
    [SerializeField] private AudioClip m_OpenClip;
    [SerializeField] private AudioClip m_ClosingClip;
    [SerializeField] private AudioClip m_CloseClip;

    [SerializeField] private Rigidbody m_DoorRb;

    private float m_LastTorque;
    private void Update()
    {
        //if (m_DoorRb.angularVelocity.magnitude > m_LastTorque)
        //{
        //    m_AudioSource.PlayOneShot(m_OpenClip);
        //}
        //else if (m_LastTorque < 0.05f && m_DoorRb.angularVelocity.magnitude > m_LastTorque)
        //{
        //    m_AudioSource.PlayOneShot(m_ClosingClip);
        //}
        //else if (m_LastTorque > m_DoorRb.angularVelocity.magnitude && m_DoorRb.angularVelocity.magnitude < 0.01f)
        //{
        //    m_AudioSource.PlayOneShot(m_CloseClip);
        //}

        //m_LastTorque = m_DoorRb.angularVelocity.magnitude;
    }
}