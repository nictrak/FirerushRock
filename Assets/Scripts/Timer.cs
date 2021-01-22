using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeLimit;
    private float timeleft;
    public TimerText TimerText;
    private int timeInt;
    public PlaySceneAudio playSceneAudio;
    public LoseCanvas LoseCanvas;
    void Start()
    {
        timeleft = timeLimit;
        timeInt = 0;
        Debug.Log(Time.fixedDeltaTime);
        //Time.fixedDeltaTime = Time.fixedDeltaTime*2;
        Debug.Log(Time.fixedDeltaTime);
    }

    void FixedUpdate()
    {



        if (timeleft <= 0)
        {
            TimeOver();
        }
        else
        {
            timeleft -= Time.deltaTime;
        }

        sentTimeToTimerText();


    }

    void sentTimeToTimerText()
    {
        if ((int)timeleft != timeInt)
        {
            timeInt = (int)timeleft;
            TimerText.ChangeText(timeInt);
        }
    }

    void TimeOver()
    {
        playSceneAudio.StopMusic();
        LoseCanvas.Lose();
    }
}
