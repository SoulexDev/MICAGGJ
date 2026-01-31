using UnityEngine;

public class PlayerIdle : State<PlayerController>
{
    public override void EnterState(PlayerController ctx)
    {
        
    }
    public override void ExitState(PlayerController ctx)
    {
        
    }
    public override void FixedUpdateState(PlayerController ctx)
    {
        
    }
    public override void UpdateState(PlayerController ctx)
    {
        if (ctx.SwitchByCondition(PlayerState.Sprint, ctx.isUsingInput && Input.GetKey(KeyCode.LeftShift)))
            return;
        if (ctx.SwitchByCondition(PlayerState.Walk, ctx.isUsingInput))
            return;
    }
}