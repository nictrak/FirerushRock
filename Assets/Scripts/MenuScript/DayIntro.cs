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
    }

    // Update is called once per frame
    void Update()
    {
        text = this.GetComponent<Text>();
        GameObject scoreObj = GameObject.FindGameObjectWithTag("Score");
        if(scoreObj != null)
        {
            int d = scoreObj.GetComponent<Score>().getDay();
            if (d <= 0)
            {
                d = 1;
            }
            text.text = "Day " + d;
        }
    }
}
