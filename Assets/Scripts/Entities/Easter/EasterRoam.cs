using UnityEngine;

public class EasterRoam : State<EasterController>
{
    private Vector3 m_LastTarget;
    public override void EnterState(EasterController ctx)
    {
        ctx.agent.isStopped = false;
        m_LastTarget = ctx.transform.position;
    }
    public override void ExitState(EasterController ctx)
    {

    }
    public override void FixedUpdateState(EasterController ctx)
    {

    }
    public override void UpdateState(EasterController ctx)
    {
        Vector2 randomPos = Random.insideUnitCircle * 8;
        Vector3 targetPos = m_LastTarget;
        targetPos.x += randomPos.x;
        targetPos.z += randomPos.y;

        ctx.agent.SetDestination(targetPos);
        m_LastTarget = targetPos;

        if (ctx.SwitchByCondition(EasterState.Chase, ctx.characterData.oppPresence > 1))
            return;
    }
}