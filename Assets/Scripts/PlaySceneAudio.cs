using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class PlaySceneAudio : NetworkBehaviour
{
    // Start is called before the first frame update
    public AudioClip High1;
    public AudioClip High2;
    public AudioClip High3;
    //public AudioClip High4;

    public AudioClip Low1;
    public AudioClip Low2;
    public AudioClip Low3;
    public AudioClip Low4;

    private AudioSource Music;

    void Start()
    {
        Music = GetComponent<AudioSource>();
        Music.volume = GameConfig.MusicVolume;
        Music.clip = randomClip(new List<AudioClip>{ Low1,Low2,Low3,Low4});
        Music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private AudioClip randomClip(List<AudioClip> list)
    {
        System.Random random = new System.Random();
        return list[random.Next(list.Count)];
    }
    [ClientRpc]
    public void ChangeMusicToHigh()
    {
        Music.Stop();
        Music.clip = randomClip(new List<AudioClip> { High1,High2,High3 });
        Music.Play();
    }
}
