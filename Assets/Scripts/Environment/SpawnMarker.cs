using UnityEngine;
using System.Collections.Generic;

public class SpawnMarker : MonoBehaviour
{
    public float spawnChance = 0.5f;
    public float mimicChance = 0.05f;
    

    // this is kinda backwards - spawn everything and then delete the ones we don't want
    // should make the layout process easier
    public void ProcessSpawn()
    {
        if(Random.Range(0f, 1f) > spawnChance)
        {
            if(Application.isPlaying)
            {
                Destroy(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }
    }

}
