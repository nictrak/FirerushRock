using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SongDropdown : MonoBehaviour
{
    public AudioClip Music1;
    public AudioClip Music2;
    public AudioClip Music3;
    public AudioClip Music4;
    public AudioClip Music5;

    private AudioClip ChoosenMusic;

    // Start is called before the first frame update
    void Start()
    {
        ChoosenMusic = Music1;
        GameConfig.Music = ChoosenMusic;

    }

    public void HandleDropdownInput(int val)
    {
        if (val+1 == 1)
        {
            ChoosenMusic = Music1;
        }
        if (val + 1 == 2)
        {
            ChoosenMusic = Music2;
        }
        if (val + 1 == 3)
        {
            ChoosenMusic = Music3;
        }
        if (val + 1 == 4)
        {
            ChoosenMusic = Music4;
        }
        if (val + 1 == 5)
        {
            ChoosenMusic = Music5;
        }

        GameConfig.Music = ChoosenMusic;
        Debug.Log("You just choose "+ ChoosenMusic.name);
    }
}
