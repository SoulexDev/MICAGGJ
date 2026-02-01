using UnityEngine;

public class EasterChase : State<EasterController>
{
    public override void EnterState(EasterController ctx)
    {
        ctx.anims.SetFloat("MoveState", 1);
        ctx.piqueSource.PlayOneShot();
    }
    public override void ExitState(EasterController ctx)
    {

    }
    public override void FixedUpdateState(EasterController ctx)
    {

    }
    public override void UpdateState(EasterController ctx)
    {
        ctx.agent.SetDestination(ctx.characterData.targetOpp.transform.position);

        ctx.characterData.heatMap += Time.deltaTime;

        if (ctx.SwitchByCondition(EasterState.Launch, ctx.characterData.targetOppDistance < 6))
            return;
    }
}