using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayIntro : MonoBehaviour
{
    // Start is called before the first frame update
    private Text text;
    void Start()
    {
        text = this.GetComponent<Text>();
        int d = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().getDay();
        if (d <= 0)
        {
            d = 1;
        }
        text.text = "Day " + d;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
