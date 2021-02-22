using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource Sfx;

    public AudioClip hover;
    public AudioClip click;
    private Image img;
    void Start()
    {
        Sfx = GetComponent<AudioSource>();
        img = GetComponent<Image>();
        img.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HoverButton()
    {
        img.enabled = true;
        Sfx.Stop();
        Sfx.clip = hover;
        Sfx.Play();
    }

    public void ExitHoverButton()
    {
        img.enabled = false;
    }

    public void ClickButton()
    {
        Sfx.Stop();
        Sfx.clip = click;
        Sfx.Play();
    }
}
