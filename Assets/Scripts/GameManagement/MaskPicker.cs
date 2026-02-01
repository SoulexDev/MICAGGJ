using UnityEngine;

public class MaskPicker : MonoBehaviour
{
    public static MaskPicker Instance;

    public MaskType chosenMask;
    public Animator playercutscene;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void PickMask(string maskType)
    {
        chosenMask = maskType switch
        {
            "Joy" => MaskType.Joy,
            "Despair" => MaskType.Despair,
            "Anger" => MaskType.Anger,
            _ => MaskType.None
        };
if(playercutscene != null)
{
playercutscene.SetTrigger("cutscene");
}
        
    }
}