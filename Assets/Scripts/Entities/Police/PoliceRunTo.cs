using UnityEngine;

public class PoliceRunTo : State<PoliceController>
{
    public override void EnterState(PoliceController ctx)
    {
        ctx.agent.isStopped = false;
    }
    public override void ExitState(PoliceController ctx)
    {

    }
    public override void FixedUpdateState(PoliceController ctx)
    {

    }
    public override void UpdateState(PoliceController ctx)
    {
        ctx.anims.SetFloat("MoveState", ctx.agent.velocity.magnitude / ctx.agent.speed);

        ctx.agent.SetDestination(ctx.characterData.targetAlly.transform.position);

        ctx.characterData.heatMap += Time.deltaTime;

        if (ctx.SwitchByCondition(PoliceState.Idle, ctx.characterData.allyPresence < 0.5f))
            return;
        if (ctx.SwitchByCondition(PoliceState.Fire, ctx.characterData.targetOppDistance < 8 && ctx.characterData.allyPresence >= 0.5f))
            return;
        if (ctx.SwitchByCondition(PoliceState.RunFrom, ctx.characterData.oppPresence > 1))
            return;
    }
}