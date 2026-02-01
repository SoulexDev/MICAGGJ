using UnityEngine;
using UnityEngine.AI;

public enum CivilianState { Idle, RunFrom, RunTo }
public class CivilianController : StateMachine<CivilianController>
{
    public NavMeshAgent agent;
    public Character characterData;

    private void Awake()
    {
        stateDictionary.Add(CivilianState.Idle, new CivilianIdle());
        stateDictionary.Add(CivilianState.RunFrom, new CivilianRunFrom());
        stateDictionary.Add(CivilianState.RunTo, new CivilianRunTo());

        SwitchState(CivilianState.Idle);
    }
    public override void Update()
    {
        characterData.heatMap = Mathf.Lerp(characterData.heatMap, 0, Time.deltaTime * 0.5f);
        base.Update();
    }
}