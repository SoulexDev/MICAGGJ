using UnityEngine;

public class PoliceFireGun : MonoBehaviour
{
    public AdvancedAudioSource audioSource;
    public PoliceController policeController;
    void PoliceFire()
    {
        audioSource.PlayOneShot();
        policeController.Fire();
    }
}
