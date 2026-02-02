using UnityEngine;

public class MimicRoam : State<MimicController>
{
    private Vector3 m_LastTarget;
    private int m_FrameCounter;
    //private float m_GrowlTimer;
    public override void EnterState(MimicController ctx)
    {
        ctx.agent.isStopped = false;
        m_LastTarget = ctx.transform.position;
        m_FrameCounter = 0;

        //m_GrowlTimer = Random.Range(4f, 7f);
    }
    public override void ExitState(MimicController ctx)
    {

    }
    public override void FixedUpdateState(MimicController ctx)
    {

    }
    public override void UpdateState(MimicController ctx)
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

        //m_GrowlTimer -= Time.deltaTime;

        //if (m_GrowlTimer <= 0)
        //{
        //    m_GrowlTimer = Random.Range(4f, 7f);
        //    ctx.slowGrowlSource.PlayOneShot();
        //}

        ctx.characterData.heatMap += Time.deltaTime;

        ctx.anims.SetFloat("MoveState", ctx.agent.velocity.magnitude / ctx.agent.speed);
        if (ctx.SwitchByCondition(MimicState.Chase, ctx.characterData.oppPresence > 1))
            return;

        m_FrameCounter++;
    }
}