using UnityEngine;
using UnityEngine.AI;

public enum CivilianState { Idle, RunFrom, RunTo, Dead }
public class CivilianController : StateMachine<CivilianController>
{
    public NavMeshAgent agent;
    public Character characterData;
    public Animator anims;

    private void Awake()
    {
        stateDictionary.Add(CivilianState.Idle, new CivilianIdle());
        stateDictionary.Add(CivilianState.RunFrom, new CivilianRunFrom());
        stateDictionary.Add(CivilianState.RunTo, new CivilianRunTo());
        stateDictionary.Add(CivilianState.Dead, new CivilianDead());

        SwitchState(CivilianState.Idle);

        characterData.OnDie += CharacterData_OnDie;
    }
    public override void Update()
    {
        characterData.heatMap = Mathf.Lerp(characterData.heatMap, 0, Time.deltaTime * 0.5f);

        if (characterData.isDead)
        {
            return;
        }
        base.Update();
    }
    private void CharacterData_OnDie()
    {
        SwitchState(CivilianState.Dead);
        anims.SetTrigger("Die");
    }
}