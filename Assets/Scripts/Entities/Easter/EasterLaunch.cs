using UnityEngine;

public class EasterLaunch : State<EasterController>
{
    private float m_PreviousSpeed;
    public override void EnterState(EasterController ctx)
    {
        ctx.anims.SetTrigger("Launch");
        ctx.agent.SetDestination(ctx.characterData.targetOpp.transform.position);

        Debug.Log($"{ctx.agent.destination}, {ctx.characterData.targetOppDirectionNormalized * 8}");

        m_PreviousSpeed = ctx.agent.speed;
        ctx.agent.speed = 32;

        ctx.fastGrowlSource.PlayOneShot();
    }
    public override void ExitState(EasterController ctx)
    {
        ctx.agent.speed = m_PreviousSpeed;
    }
    public override void FixedUpdateState(EasterController ctx)
    {

    }
    public override void UpdateState(EasterController ctx)
    {
        if (Physics.SphereCast(ctx.transform.position, 0.5f, ctx.transform.forward, 
            out RaycastHit hit, 2, GameManager.enemyRayIgnoreMask))
        {
            if (hit.transform.TryGetComponent(out IHealth health))
            {
                health.Damage(2);
            }
            Collider[] cols = Physics.OverlapSphere(hit.point, 4, GameManager.enemyRayIgnoreMask);

            foreach (var col in cols)
            {
                if (col.TryGetComponent(out Rigidbody rb))
                {
                    rb.AddExplosionForce(8, hit.point, 4, 2, ForceMode.Impulse);
                }
            }

            ctx.SwitchState(EasterState.Hurt);
            return;
        }
        ctx.SwitchByCondition(EasterState.Hurt, ctx.agent.remainingDistance < 0.5f);
    }
}