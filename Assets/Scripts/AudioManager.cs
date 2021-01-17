using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    public AudioSource BGM;
    public AudioClip BaseMusic;

    // Start is called before the first frame update
    void Start()
    {
        ChangeBGM(GameConfig.Music);
        /*
        if(GameConfig.Music == null)
        {
            ChangeBGM(BaseMusic);
        }
        */
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
