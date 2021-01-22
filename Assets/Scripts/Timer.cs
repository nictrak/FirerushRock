using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeLimit;
    private float timeleft;
    public TimerText TimerText;
    private int timeInt;
    private bool isWorking;
    public LevelScriptController LevelScriptController;
    public void startf()
    {
        isWorking = false;
        timeleft = timeLimit;
        timeInt = 0;
        Debug.Log(Time.fixedDeltaTime);
        //Time.fixedDeltaTime = Time.fixedDeltaTime*2;
        Debug.Log(Time.fixedDeltaTime);
        
    }

    public void setWorking(bool w)
    {
        isWorking = w;
    }

    void FixedUpdate()
    {
        if (isWorking) { 
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
        LevelScriptController.MissionFailed();
    }

    public int getTimeLeft()
    {
        return timeInt;
    }
}
