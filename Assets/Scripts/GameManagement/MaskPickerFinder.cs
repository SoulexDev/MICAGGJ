using UnityEngine;

public class MaskPickerFinder : MonoBehaviour
{
    public void PickMask(string maskType)
    {
        MaskPicker.Instance.PickMask(maskType);
    }
}