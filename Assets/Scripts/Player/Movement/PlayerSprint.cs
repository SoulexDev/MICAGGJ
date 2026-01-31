using UnityEngine;

public class PlayerSprint : State<PlayerController>
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
        ctx.characterController.Move(ctx.moveVector * ctx.runSpeed * Time.deltaTime);

        if (ctx.SwitchByCondition(PlayerState.Idle, !ctx.isUsingInput))
            return;
        if (ctx.SwitchByCondition(PlayerState.Walk, !Input.GetKey(KeyCode.LeftShift)))
            return;
    }
}