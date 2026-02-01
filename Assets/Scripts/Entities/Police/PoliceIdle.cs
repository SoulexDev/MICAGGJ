using UnityEngine;

public class PoliceIdle : State<PoliceController>
{
    public override void EnterState(PoliceController ctx)
    {
        ctx.anims.SetFloat("MoveState", 0);
        ctx.agent.isStopped = true;
    }
    public override void ExitState(PoliceController ctx)
    {

    }
    public override void FixedUpdateState(PoliceController ctx)
    {

    }
    public override void UpdateState(PoliceController ctx)
    {
        if (ctx.SwitchByCondition(PoliceState.Fire, ctx.characterData.allyPresence >= 0.5f && ctx.characterData.targetOppDistance < 8))
            return;
        if (ctx.SwitchByCondition(PoliceState.RunTo, ctx.characterData.allyPresence >= 0.5f && ctx.characterData.targetOppDistance > 8))
            return;
        if (ctx.SwitchByCondition(PoliceState.RunFrom, ctx.characterData.oppPresence > 1 && ctx.characterData.allyPresence < 0.5f))
            return;
    }
}