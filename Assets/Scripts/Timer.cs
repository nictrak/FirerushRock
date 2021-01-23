using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Timer : NetworkBehaviour
{
    public float timeLimit;
    private float timeleft;
    public TimerText TimerText;
    [SyncVar]
    private int timeInt;
    private bool isWorking;
    public LevelScriptController LevelScriptController;

    public int TimeInt { get => timeInt; set => timeInt = value; }

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
    [ServerCallback]
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
    [ServerCallback]
    void sentTimeToTimerText()
    {
        if ((int)timeleft != timeInt)
        {
            timeInt = (int)timeleft;
        }
    }
    [ServerCallback]
    void TimeOver()
    {
        LevelScriptController.MissionFailed();
    }

    public int getTimeLeft()
    {
        return timeInt;
    }
}
