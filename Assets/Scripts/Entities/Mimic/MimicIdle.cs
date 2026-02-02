using UnityEngine;

public class MimicIdle : State<MimicController>
{
    //TODO: Generic state machine super class with various generic states. states are assignable in editor
    public override void EnterState(MimicController ctx)
    {
        ctx.agent.isStopped = true;
        ctx.anims.SetFloat("MoveState", 0);
    }
    public override void ExitState(MimicController ctx)
    {
        
    }
    public override void FixedUpdateState(MimicController ctx)
    {
        
    }
    public override void UpdateState(MimicController ctx)
    {
        
    }
}