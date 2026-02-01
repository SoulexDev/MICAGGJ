using UnityEngine;

public class PoliceFire : State<PoliceController>
{
    private float m_FireTimer;
    public override void EnterState(PoliceController ctx)
    {
        ctx.agent.isStopped = true;
        ctx.anims.SetBool("Firing", true);
        m_FireTimer = 0.25f;
    }
    public override void ExitState(PoliceController ctx)
    {
        ctx.anims.SetBool("Firing", false);
    }
    public override void FixedUpdateState(PoliceController ctx)
    {
        
    }
    public override void UpdateState(PoliceController ctx)
    {
        Vector3 oppDir = new Vector3(ctx.characterData.targetOppDirectionNormalized.x, 0, ctx.characterData.targetOppDirectionNormalized.z);
        ctx.transform.rotation = Quaternion.LookRotation(oppDir, Vector3.up);

        if (ctx.SwitchByCondition(PoliceState.RunFrom, ctx.characterData.allyPresence < 0.5f &&
            ctx.characterData.oppPresence > 0.75f))
            return;
    }
}