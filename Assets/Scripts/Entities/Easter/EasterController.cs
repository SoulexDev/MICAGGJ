using UnityEngine;
using UnityEngine.AI;

public enum EasterState { Idle, Roam, Chase, Launch, Hurt }
public class EasterController : StateMachine<EasterController>
{
    public NavMeshAgent agent;
    public Animator anims;
    public Character characterData;

    public AdvancedAudioSource piqueSource;
    public AdvancedAudioSource slowGrowlSource;
    public AdvancedAudioSource fastGrowlSource;
    public AdvancedAudioSource hurtSource;

    private void Awake()
    {
        stateDictionary.Add(EasterState.Idle, new EasterIdle());
        stateDictionary.Add(EasterState.Roam, new EasterRoam());
        stateDictionary.Add(EasterState.Chase, new EasterChase());
        stateDictionary.Add(EasterState.Launch, new EasterLaunch());
        stateDictionary.Add(EasterState.Hurt, new EasterHurt());

        SwitchState(EasterState.Idle);

        characterData.OnDie += CharacterData_OnDie;
    }
    public override void Update()
    {
        characterData.heatMap = Mathf.Lerp(characterData.heatMap, 0, Time.deltaTime * 0.5f);
        base.Update();
    }
    private void CharacterData_OnDie()
    {
        SwitchState(EasterState.Hurt);
    }
}