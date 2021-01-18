using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSound : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource music;
    void Start()
    {
        if (GameConfig.MusicVolume == 0)
        {
            GameConfig.MusicVolume = (float)1;
        }

        Slider slider = this.gameObject.GetComponent<Slider>();
        slider.value = GameConfig.MusicVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolume(float sliderValue)
    {
        music.volume = sliderValue;
        GameConfig.MusicVolume = sliderValue;
    }
}
