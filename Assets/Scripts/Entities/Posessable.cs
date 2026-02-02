using System.Collections;
using UnityEngine;

public class Posessable : MonoBehaviour
{
    [SerializeField] private GameObject m_Mimic;
    public void Posess()
    {
        StartCoroutine(Orient());
    }
    private IEnumerator Orient()
    {
        while (Vector3.Dot(transform.up, Vector3.up) < 0.9f)
        {
            transform.up = Vector3.MoveTowards(transform.up, Vector3.up, Time.deltaTime * 15);
            yield return null;
        }

        Instantiate(m_Mimic, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}