using UnityEngine;
using UnityEngine.AI;

public enum PoliceState { Idle, RunFrom, RunTo, Fire, Dead }
public class PoliceController : StateMachine<PoliceController>
{
    public NavMeshAgent agent;
    public Character characterData;
    public Animator anims;

    private void Awake()
    {
        stateDictionary.Add(PoliceState.Idle, new PoliceIdle());
        stateDictionary.Add(PoliceState.RunFrom, new PoliceRunFrom());
        stateDictionary.Add(PoliceState.RunTo, new PoliceRunTo());
        stateDictionary.Add(PoliceState.Fire, new PoliceFire());
        stateDictionary.Add(PoliceState.Dead, new PoliceDead());

        SwitchState(PoliceState.Idle);

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
    public void Fire()
    {
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward,
                out RaycastHit hit, 999, GameManager.policeRayIgnoreMask))
        {
            print(hit.transform);
            if (hit.transform.TryGetComponent(out IHealth health))
            {
                health.Damage(1);
            }
            if (hit.transform.TryGetComponent(out Character c))
            {
                if (characterData.IsAllied(c))
                {
                    characterData.characterType = CharacterType.Nuanced;
                }
            }
            characterData.heatMap += 2;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, characterData.targetOppDirectionNormalized);
    }
    private void CharacterData_OnDie()
    {
        print("popo dead");
        SwitchState(PoliceState.Dead);
        anims.SetTrigger("Die");
    }
}