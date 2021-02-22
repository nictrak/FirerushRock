using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyButton : MonoBehaviour
{
    public Sprite Normal;
    public Sprite Hover;
    private Image Img;


    // Start is called before the first frame update
    void Start()
    {
        Img = GetComponent<Image>();
    }

    // Update is called once per frame
    public void hover()
    {
        Img.sprite = Hover;
    }

    public void exitHover()
    {
        Img.sprite = Normal;
    }
}
