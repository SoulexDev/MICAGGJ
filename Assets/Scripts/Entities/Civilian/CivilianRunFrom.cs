using UnityEngine;

public class CivilianRunFrom : State<CivilianController>
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

        Vector3 targetPosition = ctx.transform.position - ctx.characterData.targetOpp.transform.position;
        targetPosition.Normalize();
        targetPosition *= 10;

        ctx.agent.SetDestination(targetPosition);

        ctx.characterData.heatMap += Time.deltaTime;

        if (ctx.SwitchByCondition(CivilianState.Idle, ctx.characterData.allyPresence < 0.5f && ctx.characterData.oppPresence < 0.5f))
            return;
        if (ctx.SwitchByCondition(CivilianState.RunTo, ctx.characterData.allyPresence > 0.75f))
            return;
    }
}