using UnityEngine;

public class EasterIdle : State<EasterController>
{
    public override void EnterState(EasterController ctx)
    {
        
    }
    public override void ExitState(EasterController ctx)
    {
        
    }
    public override void FixedUpdateState(EasterController ctx)
    {
        
    }
    public override void UpdateState(EasterController ctx)
    {
        if (ctx.SwitchByCondition(EasterState.Chase, ctx.characterData.oppPresence > 1))
            return;
    }
}