using UnityEngine;
using UnityEngine.Video;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public PlayerController playerController;
    public Character character;

    [SerializeField] private GameObject m_VideoObj;
    [SerializeField] private VideoPlayer m_VideoPlayer;
    [SerializeField] private SceneFader m_DeathFader;

    private void Awake()
    {
        Instance = this;

        character.OnDie += Character_OnDie;
    }

    private void Character_OnDie()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_VideoObj.SetActive(true);
        m_VideoPlayer.Play();
        m_DeathFader.Fade();
    }
}