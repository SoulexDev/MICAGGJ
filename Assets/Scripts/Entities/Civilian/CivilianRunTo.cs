using UnityEngine;

public class CivilianRunTo : State<CivilianController>
{
    public override void EnterState(CivilianController ctx)
    {
        ctx.agent.isStopped = false;
    }
    public override void ExitState(CivilianController ctx)
    {
        
    }
    public override void FixedUpdateState(CivilianController ctx)
    {
        
    }
    public override void UpdateState(CivilianController ctx)
    {
        ctx.anims.SetFloat("MoveState", ctx.agent.velocity.magnitude / ctx.agent.speed);

        ctx.agent.SetDestination(ctx.characterData.targetAlly.transform.position);

        ctx.characterData.heatMap += Time.deltaTime;

        if (ctx.SwitchByCondition(CivilianState.Idle, ctx.characterData.allyPresence < 0.5f))
            return;
        if (ctx.SwitchByCondition(CivilianState.RunFrom, ctx.characterData.oppPresence > 0.75f))
            return;
    }
}