public class EnemyChaseTarget : State<EnemyController>
{

    int pathUpdateCounter = 0;
    int pathUpdateMax = 30;
    public override void EnterState(EnemyController ctx)
    {
        pathUpdateCounter = 0;
        ctx.PathToTarget();
        ctx.navAgent.isStopped = false;
    }

    public override void ExitState(EnemyController ctx)
    {
        ctx.navAgent.isStopped = true;
    }

    public override void FixedUpdateState(EnemyController ctx)
    {
        pathUpdateCounter++;
        if (pathUpdateCounter >= pathUpdateMax)
        {
            ctx.PathToTarget();
        }
        ctx.TryToAttack();
    }

    public override void UpdateState(EnemyController ctx)
    { 
    }
}