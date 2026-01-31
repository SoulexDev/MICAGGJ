using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CharacterType { Civilian = 1, Police = 2, Monster = 4 }
public enum MaskType { None = 0, Joy = 1, Despair = 2, Anger = 4 }
public class Character : MonoBehaviour, IHealth
{
    public CharacterType characterType;
    public MaskType maskType;

    public float maxHealth;
    private float m_Health;

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

    private List<Character> m_OtherHeuristics => CharacterManager.Instance.characters;
    private Dictionary<CharacterType, CharacterType> m_AllianceTable = new Dictionary<CharacterType, CharacterType>
    {
        { CharacterType.Police, CharacterType.Civilian | CharacterType.Police },
        { CharacterType.Civilian, CharacterType.Police | CharacterType.Civilian },
        { CharacterType.Monster, CharacterType.Monster }
    };
    //[Allied, Mask] => SensitivityMultiplier
    private Dictionary<(bool, MaskType), float> m_MultiplierTable = new Dictionary<(bool, MaskType), float>
    {
        { (false, MaskType.None), 1 },
        { (true, MaskType.None), 1 },

        { (false, MaskType.Joy), 2 },
        { (true, MaskType.Joy), 0.5f },

         { (false, MaskType.Despair), 0.5f },
        { (true, MaskType.Despair), 2 },

         { (false, MaskType.Anger), 1 },
        { (true, MaskType.Anger), 1 },
    };

    public delegate void Die();
    public event Die OnDie;

    public delegate void AllyDie();
    public event AllyDie OnAllyDie;

    public delegate void OppDie();
    public event OppDie OnOppDie;

    private void Awake()
    {
        m_Health = maxHealth;

        foreach (Character heu in m_OtherHeuristics)
        {
            heu.OnDie += () =>
            {
                if (heu != this && IsAllied(heu))
                {
                    OnAllyDie?.Invoke();
                }
                else if (heu != this && IsOpped(heu))
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
    public bool Damage(float amount)
    {
        m_Health -= amount;
        isShot += 1;

        isShot = Mathf.Clamp(isShot, 0, 1);

        return true;
    }
    public float GetShotAtAlly()
    {
        float value = 0;
        foreach (Character heu in m_OtherHeuristics)
        {
            if (heu != this && IsAllied(heu))
            {
                value = Mathf.Max(heu.isShot * m_MultiplierTable[(true, heu.maskType)]);
            }
        }
        return value;
    }
    public float GetShotAtOpp()
    {
        float value = 0;
        foreach (Character heu in m_OtherHeuristics)
        {
            if (heu != this && IsOpped(heu) && !heu.isDead)
            {
                value = Mathf.Max(heu.isShot * m_MultiplierTable[(false, heu.maskType)]);
            }
        }
        return value;
    }
    public float GetGunPointedMe()
    {
        float value = 0;
        foreach (Character heu in m_OtherHeuristics)
        {
            if (heu != this && IsAllied(heu) && !heu.isDead)
            {
                float compareValue = Vector3.Dot(heu.lookDirection, (transform.position - heu.transform.position).normalized);
                value = Mathf.Max(value, compareValue * m_MultiplierTable[(true, heu.maskType)]);
            }
        }
        return value;
    }
    public float GetGunPointedAlly()
    {
        float value = 0;
        foreach (Character heu in m_OtherHeuristics)
        {
            if (heu != this && IsAllied(heu) && !heu.isDead)
            {
                value = Mathf.Max(value, heu.GetGunPointedMe() * m_MultiplierTable[(true, heu.maskType)]);
            }
        }
        return value;
    }
    public float GetGunPointedOpp()
    {
        float value = 0;
        foreach (Character heu in m_OtherHeuristics)
        {
            if (heu != this && IsOpped(heu) && !heu.isDead)
            {
                value = Mathf.Max(value, heu.GetGunPointedMe() * m_MultiplierTable[(false, heu.maskType)]);
            }
        }
        return value;
    }
    public float GetAllyPresence()
    {
        if (m_OtherHeuristics.Count > 0 && m_OtherHeuristics.Exists(h => IsAllied(h) && !h.isDead))
        {
            Character heu = m_OtherHeuristics.OrderBy(h => Vector3.Distance(h.transform.position, transform.position)).First(h => h != this && IsAllied(h));
            float dist = Vector3.Distance(transform.position, heu.transform.position);
            return heu.heatMap * Mathf.Clamp01(1f - dist / 15f) * m_MultiplierTable[(true, heu.maskType)];
        }
        else
            return 0;
    }
    public float GetOppPresence()
    {
        if (m_OtherHeuristics.Count > 0 && m_OtherHeuristics.Exists(h => IsOpped(h) && !h.isDead))
        {
            Character heu = m_OtherHeuristics.OrderBy(h => Vector3.Distance(h.transform.position, transform.position)).First(h => h != this && IsOpped(h));
            float dist = Vector3.Distance(transform.position, heu.transform.position);
            return heu.heatMap * Mathf.Clamp01(1f - dist / 15f) * m_MultiplierTable[(false, heu.maskType)];
        }
        else
            return 0;
    }
    private Vector3 GetNearestOpp()
    {
        if (m_OtherHeuristics.Count > 0 && m_OtherHeuristics.Exists(h => IsOpped(h) && !h.isDead))
        {
            Character heu = m_OtherHeuristics.OrderBy(h => Vector3.Distance(h.transform.position, transform.position)).First(h => h != this && IsOpped(h));
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