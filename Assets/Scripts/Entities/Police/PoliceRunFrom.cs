using UnityEngine;

public class PoliceRunFrom : State<PoliceController>
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

        Vector3 targetPosition = ctx.transform.position - ctx.characterData.targetOpp.transform.position;
        targetPosition.Normalize();
        targetPosition *= 10;

        ctx.agent.SetDestination(targetPosition);

        ctx.characterData.heatMap += Time.deltaTime;

        if (ctx.SwitchByCondition(PoliceState.Idle, ctx.characterData.allyPresence < 0.5f && ctx.characterData.oppPresence < 0.75f))
            return;
        if (ctx.SwitchByCondition(PoliceState.Fire, ctx.characterData.targetOppDistance < 8 && ctx.characterData.allyPresence >= 0.5f))
            return;
        if (ctx.SwitchByCondition(PoliceState.RunTo, ctx.characterData.allyPresence >= 0.5f))
            return;
    }
}