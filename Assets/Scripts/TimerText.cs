using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimerText : MonoBehaviour
{
    // Start is called before the first frame update
    private Text text;
    public Timer timer;
    
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeText();
    }
    public void ChangeText()
    {
        text = GetComponent<Text>();
        text.text = "" + timer.TimeInt;
    }
}
