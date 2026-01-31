using UnityEngine;

public class EnemyLookForTarget : State<EnemyController>
{
    public override void EnterState(EnemyController ctx)
    {
    }

    public override void ExitState(EnemyController ctx)
    {
    }

    public override void FixedUpdateState(EnemyController ctx)
    {
    }

    public override void UpdateState(EnemyController ctx)
    {
        var player = ctx.GetPlayer();

        float dist = Vector3.Distance(player.transform.position, ctx.transform.position);

        Vector3 dir = (player.transform.position + Vector3.up) - ctx.transform.position;

        Debug.DrawRay(ctx.transform.position, dir);

        if (dist <= ctx.enemy.aggroRange && 
            Physics.Raycast(ctx.transform.position, dir, 
            out RaycastHit hit, ctx.enemy.aggroRange))
        {
            Debug.Log(hit.transform.gameObject);
            if(hit.transform.gameObject == player.gameObject)
            {
                ctx.target = player.gameObject;
                ctx.SwitchState(EnemyStates.ChaseTarget);
            }
        }
    }
}