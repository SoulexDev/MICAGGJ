using UnityEngine;

public class CivilianDead : State<CivilianController>
{
    public override void EnterState(CivilianController ctx)
    {
        ctx.agent.isStopped = true;
    }
    public override void ExitState(CivilianController ctx)
    {

    }
    public override void FixedUpdateState(CivilianController ctx)
    {

    }
    public override void UpdateState(CivilianController ctx)
    {
        
    }
}