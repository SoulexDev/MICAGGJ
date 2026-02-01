using UnityEngine;

public class EasterRoam : State<EasterController>
{
    private Vector3 m_LastTarget;
    private int m_FrameCounter;
    private float m_GrowlTimer;
    public override void EnterState(EasterController ctx)
    {
        ctx.agent.isStopped = false;
        m_LastTarget = ctx.transform.position;
        m_FrameCounter = 0;

        m_GrowlTimer = Random.Range(4f, 7f);
    }
    public override void ExitState(EasterController ctx)
    {

    }
    public override void FixedUpdateState(EasterController ctx)
    {

    }
    public override void UpdateState(EasterController ctx)
    {
        if (m_FrameCounter % 4 == 0)
        {
            if (Vector3.Distance(m_LastTarget, ctx.transform.position) > 128)
                m_LastTarget = ctx.transform.position;

            Vector2 randomPos = Random.insideUnitCircle * 8;
            Vector3 targetPos = m_LastTarget;
            targetPos.x += randomPos.x;
            targetPos.z += randomPos.y;

            targetPos = (m_LastTarget + targetPos) * 0.5f;

            ctx.agent.SetDestination(targetPos);
            m_LastTarget = targetPos;
        }

        m_GrowlTimer -= Time.deltaTime;

        if (m_GrowlTimer <= 0)
        {
            m_GrowlTimer = Random.Range(4f, 7f);
            ctx.slowGrowlSource.PlayOneShot();
        }
        ctx.anims.SetFloat("MoveState", ctx.agent.velocity.magnitude / ctx.agent.speed);
        if (ctx.SwitchByCondition(EasterState.Chase, ctx.characterData.oppPresence > 1))
            return;

        m_FrameCounter++;
    }
}