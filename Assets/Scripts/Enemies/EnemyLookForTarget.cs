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

        if (dist <= ctx.enemy.aggroRange)
        {
            ctx.target = player.gameObject;
            ctx.SwitchState(EnemyStates.ChaseTarget);
        }
    }
}