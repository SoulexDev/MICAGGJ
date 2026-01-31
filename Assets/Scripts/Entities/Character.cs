using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CharacterType { Civilian = 1, Police = 2, Monster = 4 }
public class Character : MonoBehaviour
{
    public CharacterType characterType;

    public Vector3 lookDirection;

    public Vector3 targetOpp => GetNearestOpp();
    public Vector3 targetDirection => targetOpp - transform.position;
    public Vector3 targetDirectionNormalized => targetDirection.normalized;
    public float targetDistance => targetDirection.magnitude;

    private bool _isDead;
    public bool isDead
    {
        get { return _isDead; }
        set 
        {
            _isDead = value;
            if (_isDead)
                OnDie?.Invoke();
        }
    }
    public float isShot;
    public float heatMap;

    private List<Character> m_OtherHeuristics;
    private Dictionary<CharacterType, CharacterType> m_AllianceTable = new Dictionary<CharacterType, CharacterType>
    {
        { CharacterType.Police, CharacterType.Civilian | CharacterType.Police },
        { CharacterType.Civilian, CharacterType.Police | CharacterType.Civilian },
        { CharacterType.Monster, CharacterType.Monster }
    };

    public delegate void Die();
    public event Die OnDie;

    public delegate void AllyDie();
    public event AllyDie OnAllyDie;

    public delegate void OppDie();
    public event OppDie OnOppDie;

    private void Awake()
    {
        m_OtherHeuristics = FindObjectsByType<Character>
            (FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();

        if (m_OtherHeuristics.Contains(this))
            m_OtherHeuristics.Remove(this);

        foreach (Character heu in m_OtherHeuristics)
        {
            heu.OnDie += () =>
            {
                if (IsAllied(heu))
                {
                    OnAllyDie?.Invoke();
                }
                else if (IsOpped(heu))
                {
                    OnOppDie?.Invoke();
                }
            };
        }
    }
    private void Update()
    {
        isShot = Mathf.MoveTowards(isShot, 0, Time.deltaTime * 4);
    }
    public float GetShotAtAlly()
    {
        float value = 0;
        foreach (Character heu in m_OtherHeuristics)
        {
            if (IsAllied(heu))
            {
                value = Mathf.Max(heu.isShot);
            }
        }
        return value;
    }
    public float GetShotAtOpp()
    {
        float value = 0;
        foreach (Character heu in m_OtherHeuristics)
        {
            if (IsOpped(heu) && !heu.isDead)
            {
                value = Mathf.Max(heu.isShot);
            }
        }
        return value;
    }
    public float GetGunPointedMe()
    {
        float value = 0;
        foreach (Character heu in m_OtherHeuristics)
        {
            if (IsAllied(heu) && !heu.isDead)
            {
                value = Mathf.Max(value, 
                    Vector3.Dot(heu.lookDirection, (transform.position - heu.transform.position).normalized));
            }
        }
        return value;
    }
    public float GetGunPointedAlly()
    {
        float value = 0;
        foreach (Character heu in m_OtherHeuristics)
        {
            if (IsAllied(heu) && !heu.isDead)
            {
                value = Mathf.Max(value, heu.GetGunPointedMe());
            }
        }
        return value;
    }
    public float GetGunPointedOpp()
    {
        float value = 0;
        foreach (Character heu in m_OtherHeuristics)
        {
            if (IsOpped(heu) && !heu.isDead)
            {
                value = Mathf.Max(value, heu.GetGunPointedMe());
            }
        }
        return value;
    }
    public float GetAllyPresence()
    {
        if (m_OtherHeuristics.Count > 0 && m_OtherHeuristics.Exists(h => IsAllied(h) && !h.isDead))
        {
            Character heu = m_OtherHeuristics.OrderBy(h => Vector3.Distance(h.transform.position, transform.position)).First();
            float dist = Vector3.Distance(transform.position, heu.transform.position);
            return heu.heatMap * Mathf.Clamp01(1f - dist / 15f);
        }
        else
            return 0;
    }
    public float GetOppPresence()
    {
        if (m_OtherHeuristics.Count > 0 && m_OtherHeuristics.Exists(h => IsOpped(h) && !h.isDead))
        {
            Character heu = m_OtherHeuristics.OrderBy(h => Vector3.Distance(h.transform.position, transform.position)).First();
            float dist = Vector3.Distance(transform.position, heu.transform.position);
            return heu.heatMap * Mathf.Clamp01(1f - dist / 15f);
        }
        else
            return 0;
    }
    private Vector3 GetNearestOpp()
    {
        if (m_OtherHeuristics.Count > 0 && m_OtherHeuristics.Exists(h => IsOpped(h) && !h.isDead))
        {
            Character heu = m_OtherHeuristics.OrderBy(h => Vector3.Distance(h.transform.position, transform.position)).First();
            return heu.transform.position;
        }
        else
            return Vector3.zero;
    }
    private bool IsAllied(Character heu)
    {
        return (m_AllianceTable[characterType] & heu.characterType) != 0;
    }
    private bool IsOpped(Character heu)
    {
        return (m_AllianceTable[characterType] & heu.characterType) == 0;
    }
}