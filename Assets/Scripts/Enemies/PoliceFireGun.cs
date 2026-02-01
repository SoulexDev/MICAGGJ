using UnityEngine;

public class PoliceFireGun : MonoBehaviour
{
    public AudioSource audioSrc;
    public AudioClip snd;

    void PoliceFire()
    {
        Debug.Log("FirePoliceGun");

        audioSrc.Play();
    }
}
