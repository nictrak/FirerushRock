using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    public AudioSource BGM;
    // Start is called before the first frame update
    void Start()
    {
        BGM.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeBGM(AudioClip Music)
    {
        BGM.Stop();
        BGM.clip = Music;
        BGM.Play();

    }

    public void StopBGM()
    {
        BGM.Stop();
    }

    public void PlayBGM()
    {
        BGM.Play();
    }
}
