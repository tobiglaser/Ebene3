using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    ParticleSystem Effect;

    // Update is called once per frame
    void Update()
    {
        if(!Effect.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
