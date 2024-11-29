using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstParticleDestruction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //destroy gameObject when burst particle system has finished
        ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();
        var main = particleSystem.main;
        Destroy(gameObject, main.duration);
    }
}
