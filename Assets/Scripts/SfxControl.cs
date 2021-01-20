using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SfxControl : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource sound;
    void Start()
    {
        sound = GetComponent<AudioSource>();
        sound.volume = GameConfig.SfxVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
