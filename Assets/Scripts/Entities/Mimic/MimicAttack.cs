using UnityEngine;

public class MimicAttack : State<MimicController>
{
    private float m_AttackTimer;
    public override void EnterState(MimicController ctx)
    {
        ctx.agent.isStopped = true;
        m_AttackTimer = 1;

        Collider[] cols = Physics.OverlapSphere(ctx.transform.position, 2);

        foreach (Collider col in cols)
        {
            if (col.gameObject != ctx.gameObject && col.TryGetComponent(out IHealth health))
            {
                health.Damage(0.5f);
            }
        }
    }
    public override void ExitState(MimicController ctx)
    {

    }
    public override void FixedUpdateState(MimicController ctx)
    {

    }
    public override void UpdateState(MimicController ctx)
    {
        m_AttackTimer -= Time.deltaTime;

        if (ctx.SwitchByCondition(MimicState.Roam, m_AttackTimer <= 0))
            return;
    }
}