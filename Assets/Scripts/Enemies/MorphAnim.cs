using UnityEngine;
using System.Collections;
using UnityEngine.Video;


public class MorphAnim : MonoBehaviour
{
public bool rollrandom;
    public SkinnedMeshRenderer msh;
    public float rolltimer;
    public float waitcap = 10f;
    public float restart = 2.5f;
    public Animator anim;
   
    [SerializeField] private GameObject m_VideoObj;
    [SerializeField] private VideoPlayer m_VideoPlayer;
    [SerializeField] private SceneFader m_DeathFader;

   void Update()
   {
       if(rollrandom)
       {
rolltimer+=Time.deltaTime;

if(rolltimer >= waitcap){
    // rolltimer=0f;
this.transform.position = this.transform.position + new Vector3(0,0,-.014f);
anim.SetTrigger("morph");
}

if(rolltimer >= waitcap + restart)
{

        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
        // m_VideoObj.SetActive(true);
        // m_VideoPlayer.Play();
        // m_DeathFader.Fade();
}
       }



   }
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
