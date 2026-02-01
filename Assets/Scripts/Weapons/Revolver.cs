using System.Collections;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    [SerializeField] private AdvancedAudioSource m_AudioSource;
    [SerializeField] private AudioClip m_FireClip;
    [SerializeField] private AudioClip m_DryFireClip;

    private int m_BulletIndex;
    private int m_CurrentIndex;

    private bool m_OnCooldown = false;
    private void Update()
    {
        if (!m_OnCooldown && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (m_CurrentIndex == m_BulletIndex)
            {
                Fire();
                m_BulletIndex = Random.Range(0, 6);
            }
            else
            {
                m_AudioSource.PlayOneShot(m_DryFireClip);
            }

            m_CurrentIndex++;

            if (m_CurrentIndex >= 6)
                m_CurrentIndex = 0;

            StartCoroutine(Cooldown());
        }
    }
    private void Fire()
    {
        m_AudioSource.PlayOneShot(m_FireClip);

        Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);

        if (Physics.SphereCast(ray, 0.1f, out RaycastHit hit, 999, GameManager.playerRayIgnoreMask))
        {
            if (hit.transform.TryGetComponent(out IHealth health))
            {
                health.Damage(1);
            }
            if (hit.transform.TryGetComponent(out Character c))
            {
                if (Player.Instance.character.IsAllied(c))
                {
                    Player.Instance.character.characterType = CharacterType.Nuanced;
                }
            }
        }
    }
    IEnumerator Cooldown()
    {
        m_OnCooldown = true;
        yield return new WaitForSeconds(0.25f);
        m_OnCooldown = false;
    }
}