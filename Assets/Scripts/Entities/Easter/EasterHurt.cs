using UnityEngine;

public class EasterHurt : State<EasterController>
{
    private float timer;
    public override void EnterState(EasterController ctx)
    {
        ctx.anims.SetTrigger("Stun");
        ctx.agent.isStopped = true;

        timer = 3;
    }
    public override void ExitState(EasterController ctx)
    {

    }
    public override void FixedUpdateState(EasterController ctx)
    {

    }
    public override void UpdateState(EasterController ctx)
    {
        timer -= Time.deltaTime;

        ctx.SwitchByCondition(EasterState.Roam, timer <= 0);
    }
}