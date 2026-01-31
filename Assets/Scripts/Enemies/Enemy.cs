using System;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class Enemy
{

    public float speed = 5f;
    public float aggroRange = 10f;
    public float attackCd = 1;
    public float damage = 1;
    public float attackRange = 1.5f;

}