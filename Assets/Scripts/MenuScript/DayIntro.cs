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
        text.text = "Day " + GameConfig.Day;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
