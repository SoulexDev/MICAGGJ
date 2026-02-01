using UnityEngine;
using UnityEngine.AI;

public enum EasterState { Idle, Roam, Chase, Launch, Hurt }
public class EasterController : StateMachine<EasterController>
{
    public NavMeshAgent agent;
    public Character characterData;

    private void Awake()
    {
        stateDictionary.Add(EasterState.Idle, new EasterIdle());
        stateDictionary.Add(EasterState.Roam, new EasterRoam());
        stateDictionary.Add(EasterState.Chase, new EasterChase());
        stateDictionary.Add(EasterState.Launch, new EasterLaunch());
        stateDictionary.Add(EasterState.Hurt, new EasterHurt());

        SwitchState(EasterState.Idle);
    }
}