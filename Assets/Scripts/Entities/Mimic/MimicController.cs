using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum MimicState { Idle, Roam, Chase, Attack }
public class MimicController : StateMachine<MimicController>
{
    [SerializeField] private SkinnedMeshRenderer m_SkinMesh;
    public NavMeshAgent agent;
    public Character characterData;
    public Animator anims;

    private void Awake()
    {
        stateDictionary.Add(MimicState.Idle, new MimicIdle());
        stateDictionary.Add(MimicState.Roam, new MimicRoam());
        stateDictionary.Add(MimicState.Chase, new MimicChase());
        stateDictionary.Add(MimicState.Attack, new MimicAttack());

        StartCoroutine(Morph());

        characterData.OnDie += CharacterData_OnDie;
    }
    private void CharacterData_OnDie()
    {
        Destroy(gameObject);
    }
    public override void Update()
    {
        base.Update();
    }
    private IEnumerator Morph()
    {
        SwitchState(MimicState.Idle);
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            m_SkinMesh.SetBlendShapeWeight(0, timer * 100f);
            yield return null;
        }
        m_SkinMesh.SetBlendShapeWeight(0, 100f);
        SwitchState(MimicState.Roam);
    }
}