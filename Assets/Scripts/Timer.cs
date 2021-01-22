using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeLimit;
    private float timeleft;
    public TimerText TimerText;
    private int timeInt;
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
            Debug.Log("Time out" + timeleft);
        }
        else
        {
            timeleft -= Time.deltaTime;
        }

        if((int)timeleft != timeInt)
        {
            timeInt = (int)timeleft;
            TimerText.ChangeText(timeInt);
            Debug.Log(timeInt);
        }


    }
}
