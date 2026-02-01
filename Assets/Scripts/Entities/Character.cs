using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CharacterType { Civilian = 1, Police = 2, Monster = 4 }
public enum MaskType { None = 0, Joy = 1, Despair = 2, Anger = 4 }
public class Character : MonoBehaviour, IHealth
{
    public bool isPlayer = false;

    public CharacterType characterType;
    public MaskType maskType;

    public float maxHealth;
    private float m_Health;

    public Vector3 lookDirection;

    public Character targetOpp;
    public Vector3 targettOppDirection => targetOpp != null ? targetOpp.transform.position - transform.position : Vector3.zero;
    public Vector3 targettOppDirectionNormalized => targettOppDirection.normalized;
    public float targettOppDistance => targettOppDirection.magnitude;

    public Character targetAlly;
    public Vector3 targetAllyDirection => targetAlly != null ? targetAlly.transform.position - transform.position : Vector3.zero;
    public Vector3 targetAllyDirectionNormalized => targetAllyDirection.normalized;
    public float targetAllyDistance => targetAllyDirection.magnitude;

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

    public float shotAtAlly;
    public float shotAtOpp;
    public float gunPointedMe;
    public float gunPointedAlly;
    public float gunPointedOpp;
    public float allyPresence;
    public float oppPresence;

    public float isShot;
    public float heatMap;

    private int frameTicker = 0;

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

        if (isPlayer)
            maskType = MaskPicker.Instance.chosenMask;

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
        heatMap = Mathf.MoveTowards(heatMap, 0, Time.deltaTime * 2);

        frameTicker++;

        if (frameTicker % 20 == 0)
        {
            frameTicker = 0;
            SlowUpdate();
        }
    }
    void SlowUpdate()
    {
        targetOpp = GetNearestOpp();
        targetAlly = GetNearestAlly();
        shotAtAlly = GetShotAtAlly();
        shotAtOpp = GetShotAtOpp();
        gunPointedMe = GetGunPointedMe();
        gunPointedAlly = GetGunPointedAlly();
        gunPointedOpp = GetGunPointedOpp();

        allyPresence = targetAlly ? GetAllyPresence(targetAlly) : 0;
        oppPresence = targetOpp ? GetOppPresence(targetOpp) : 0;
    }
    public bool Damage(float amount)
    {
        m_Health -= amount;
        isShot += 1;

        isShot = Mathf.Clamp(isShot, 0, 1);

        return true;
    }
    private float GetShotAtAlly()
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
    private float GetShotAtOpp()
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
    private float GetGunPointedMe()
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
    private float GetGunPointedAlly()
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
    private float GetGunPointedOpp()
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
    private float GetAllyPresence(Character heu)
    {
        float dist = Vector3.Distance(transform.position, heu.transform.position);
        return heu.heatMap * Mathf.Clamp01(1f - dist / 15f) * m_MultiplierTable[(true, heu.maskType)];
    }
    private float GetOppPresence(Character heu)
    {
        float dist = Vector3.Distance(transform.position, heu.transform.position);
        return heu.heatMap * Mathf.Clamp01(1f - dist / 15f) * m_MultiplierTable[(false, heu.maskType)];
    }
    private Character GetNearestOpp()
    {
        if (m_OtherHeuristics.Count > 0 && m_OtherHeuristics.Exists(h => IsOpped(h) && !h.isDead))
        {
            float dist = float.PositiveInfinity;
            Character heu = null;
            foreach (var c in m_OtherHeuristics)
            {
                if (c != this && !c.isDead && IsOpped(c))
                {
                    float distance = Vector3.Distance(c.transform.position, transform.position);
                    if (distance < dist)
                    {
                        heu = c;
                        dist = distance;
                    }
                }
            }
            return heu;
        }
        else
            return null;
    }
    private Character GetNearestAlly()
    {
        if (m_OtherHeuristics.Count > 0 && m_OtherHeuristics.Exists(h => IsAllied(h) && !h.isDead))
        {
            float dist = float.PositiveInfinity;
            Character heu = null;
            foreach (var c in m_OtherHeuristics)
            {
                if (c != this && !c.isDead && IsAllied(c))
                {
                    float distance = Vector3.Distance(c.transform.position, transform.position);
                    if (distance < dist)
                    {
                        heu = c;
                        dist = distance;
                    }
                }
            }
            return heu;
        }
        else
            return null;
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