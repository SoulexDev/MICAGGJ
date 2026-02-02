using UnityEngine;

public class MaskPickerFinder : MonoBehaviour
{
    public Animator playercutscene;
    public void PickMask(string maskType)
    {
        MaskPicker.Instance.PickMask(maskType);
        if (playercutscene != null)
        {
            playercutscene.SetTrigger("cutscene");
        }
    }
}