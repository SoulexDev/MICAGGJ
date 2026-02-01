using UnityEngine;

public class AnimationSoundEventRedirector : MonoBehaviour
{
    [SerializeField] private AdvancedAudioSource m_AudioSource;

    public void PlaySound()
    {
        m_AudioSource.PlayOneShot();
    }
}