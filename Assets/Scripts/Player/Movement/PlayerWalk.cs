using UnityEngine;

public class PlayerWalk : State<PlayerController>
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
        ctx.characterController.Move(ctx.moveVector * ctx.walkSpeed * Time.deltaTime);

        ctx.characterData.heatMap += Time.deltaTime;

        if (ctx.SwitchByCondition(PlayerState.Idle, !ctx.isUsingInput))
            return;
        if (ctx.SwitchByCondition(PlayerState.Sprint, Input.GetKey(KeyCode.LeftShift)))
            return;
    }
}