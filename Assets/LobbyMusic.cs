using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LobbyMusic : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource music;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        music = GetComponent<AudioSource>();
        music.volume = GameConfig.MusicVolume;
    }
}
