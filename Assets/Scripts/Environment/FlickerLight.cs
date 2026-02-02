using System.Collections;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    [SerializeField] private Light m_Light;
    private float m_LightIntensity;
    private void Awake()
    {
        m_LightIntensity = m_Light.intensity;
    }
    public void StartFlicker()
    {
        StartCoroutine(Flicker());
    }
    public void EndFlicker()
    {
        m_Light.intensity = m_LightIntensity;
        StopAllCoroutines();
    }
    IEnumerator Flicker()
    {
        float intensity;
        float intermediate;
        while (true)
        {
            intensity = 0;
            for (int i = 0; i < 3; i++)
            {
                intermediate = Mathf.Sin(Time.time * (i * 0.3f)) * (1f / i + 0.2f);
                intensity += intermediate * 0.5f + 0.5f;
            }
            m_Light.intensity = intensity * m_LightIntensity;
            yield return null;
        }
    }
}