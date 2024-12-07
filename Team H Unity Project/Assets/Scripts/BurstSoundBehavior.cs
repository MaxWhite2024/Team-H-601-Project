using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstSoundBehavior : MonoBehaviour
{
    void Awake()
    {
        //destroy gameObject when burst audiosource clip has finished
        Destroy(gameObject, gameObject.GetComponent<AudioSource>().clip.length);
    }
}
