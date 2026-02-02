using UnityEngine;

public class MimicChase : State<MimicController>
{
    public override void EnterState(MimicController ctx)
    {
        ctx.anims.SetFloat("MoveState", 1);
        ctx.agent.isStopped = false;
        //ctx.piqueSource.PlayOneShot();
    }
    public override void ExitState(MimicController ctx)
    {

    }
    public override void FixedUpdateState(MimicController ctx)
    {

    }
    public override void UpdateState(MimicController ctx)
    {
        ctx.agent.SetDestination(ctx.characterData.targetOpp.transform.position);

        ctx.characterData.heatMap += Time.deltaTime;

        if (ctx.SwitchByCondition(MimicState.Attack, ctx.characterData.targetOppDistance < 1))
            return;
    }
}