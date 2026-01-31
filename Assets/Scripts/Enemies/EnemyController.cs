
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates
{
    LookForTarget, ChaseTarget
}

public class EnemyController : StateMachine<EnemyController>
{

    public Enemy enemy;

    private PlayerController player;

    public NavMeshAgent navAgent;

    public GameObject target;

    private float nextAttack;

    private void Awake()
    {
        stateDictionary.Add(EnemyStates.LookForTarget, new EnemyLookForTarget());
        stateDictionary.Add(EnemyStates.ChaseTarget, new EnemyChaseTarget());

        navAgent = GetComponent<NavMeshAgent>() ?? gameObject.AddComponent<NavMeshAgent>();
        navAgent.speed = enemy.speed;
    }

    private void Start()
    {
        SwitchState(EnemyStates.LookForTarget);
    }

    public void PathToTarget()
    {
        navAgent.SetDestination(target.transform.position);
    }

    public PlayerController GetPlayer()
    {
        if(player == null)
        {
            player = FindFirstObjectByType<PlayerController>();
        }
        return player;
    }

    public void TryToAttack()
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);
        if(dist <= enemy.attackRange)
        {
            navAgent.isStopped = true;
            if (Time.time >= nextAttack)
            {
                nextAttack = Time.time + enemy.attackCd;
                Debug.LogWarning("ATTACK!");
            }
        }
        else
        {
            navAgent.isStopped = false;
        }

    }


}