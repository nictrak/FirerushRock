using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public int IntroFrame;
    public float ColorChangeRate;
    public float FadeRate;
    private int count;
    private Image img;
    private int Phase;
    void Start()
    {
        count = 0;
        Phase = 1;
        img = GameObject.Find("IntroPanel").GetComponent<Image>();
        img.color = Color.black;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(count >= IntroFrame & Phase == 1)
        {
            img.color = new Color(img.color[0] +  ColorChangeRate, img.color[1] + ColorChangeRate, img.color[2] + ColorChangeRate, 1);
            if (img.color == new Color(1, 1, 1, 1))
            {
                Phase = 2;
            }
        }
        if (Phase == 2)
        {
            img.color = new Color(1, 1, 1, img.color[3] - FadeRate);
            
            if (img.color[3] <= 0)
            {
                Destroy(this.gameObject);
            }
        }
        count++;
    }
}
