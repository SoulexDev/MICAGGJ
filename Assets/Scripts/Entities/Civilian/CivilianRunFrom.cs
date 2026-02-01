using UnityEngine;

public class CivilianRunFrom : State<CivilianController>
{
    public override void EnterState(CivilianController ctx)
    {
        
    }
    public override void ExitState(CivilianController ctx)
    {
        
    }
    public override void FixedUpdateState(CivilianController ctx)
    {
        
    }
    public override void UpdateState(CivilianController ctx)
    {
        Vector3 targetPosition = ctx.transform.position - ctx.characterData.targetOpp.transform.position;
        targetPosition.Normalize();
        targetPosition *= 10;

        ctx.agent.SetDestination(targetPosition);

        if (ctx.SwitchByCondition(CivilianState.Idle, ctx.characterData.allyPresence < 0.5f))
            return;
        if (ctx.SwitchByCondition(CivilianState.RunFrom, ctx.characterData.oppPresence > 0.75f))
            return;
    }
}