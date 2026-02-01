using UnityEngine;

public class PoliceDead : State<PoliceController>
{
    public override void EnterState(PoliceController ctx)
    {
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

    }
}
