using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum FadeType { FadeIn, FadeOut }
public class SceneFader : MonoBehaviour
{
    public CanvasGroup fadePanel;
    public FadeType fadeType;
    public float fadeSpeed = 0.5f;
    public float fadeDelay = 0;
    public string sceneToSwitchTo;
    public AudioMixer mixer;

    public UnityEvent prefadeEvent;

    private void Awake()
    {
        if (fadeType == FadeType.FadeOut)
            Fade();
    }
    public void Fade()
    {
        switch (fadeType)
        {
            case FadeType.FadeIn:
                StartCoroutine(FadeIn());
                break;
            case FadeType.FadeOut:
                StartCoroutine(FadeOut());
                break;
            default:
                break;
        }
    }
    IEnumerator FadeIn()
    {
        mixer.SetFloat("MusicVolume", LinearToLog(0));
        mixer.SetFloat("SFXVolume", LinearToLog(0));

        prefadeEvent?.Invoke();
        yield return new WaitForSeconds(fadeDelay);

        fadePanel.gameObject.SetActive(true);

        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime * fadeSpeed;
            fadePanel.alpha = timer;
            yield return null;
        }
        fadePanel.alpha = 1;

        SceneManager.LoadScene(sceneToSwitchTo);
    }
    IEnumerator FadeOut()
    {
        mixer.SetFloat("MasterVolume", LinearToLog(0));
        mixer.SetFloat("SFXVolume", LinearToLog(1));
        mixer.SetFloat("MusicVolume", LinearToLog(1));

        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime * fadeSpeed;
            fadePanel.alpha = 1 - timer;
            mixer.SetFloat("MasterVolume", LinearToLog(timer));
            yield return null;
        }
        fadePanel.alpha = 0;
        fadePanel.gameObject.SetActive(false);
        mixer.SetFloat("MasterVolume", LinearToLog(1));
    }
    public float LinearToLog(float value)
    {
        return value != 0 ? Mathf.Log10(value) * 20 : -144f;
    }
}