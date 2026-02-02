using System.Collections;
using UnityEngine;

public class Posessor : MonoBehaviour
{
    [SerializeField] private AdvancedAudioSource m_AudioSource;
    private Posessable m_Posessable;
    public void Initialize(Posessable target)
    {
        m_Posessable = target;
        m_AudioSource.PlayOneShot();
        StartCoroutine(Posess());
    }
    IEnumerator Posess()
    {
        Vector3 startPos = transform.position;
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(startPos, m_Posessable.transform.position, timer);
            transform.rotation = Quaternion.LookRotation((m_Posessable.transform.position - transform.position).normalized);
            yield return null;
        }

        transform.position = m_Posessable.transform.position;
        m_Posessable.Posess();
        Destroy(gameObject);
    }
}