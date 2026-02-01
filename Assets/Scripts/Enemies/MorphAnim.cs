using UnityEngine;
using System.Collections;

public class MorphAnim : MonoBehaviour
{

    public SkinnedMeshRenderer msh;
   
    void Morph()
    {
        StartCoroutine(MorphBlend());
    }

    IEnumerator MorphBlend()
    {
        float t = 0f;
       
        while(t < 1f)
        {
            t+= Time.deltaTime;

            msh.SetBlendShapeWeight(0,t * 100f);
            yield return null;
        }
    }
}
