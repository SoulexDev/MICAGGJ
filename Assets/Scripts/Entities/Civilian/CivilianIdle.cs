using UnityEngine;

public class CivilianIdle : State<CivilianController>
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
        if (ctx.SwitchByCondition(CivilianState.RunFrom, ctx.characterData.oppPresence > 0.75f))
            return;
        if (ctx.SwitchByCondition(CivilianState.RunTo, ctx.characterData.allyPresence > 0.75f))
            return;
    }
}