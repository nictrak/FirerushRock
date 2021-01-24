using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Score : NetworkBehaviour
{
    // Start is called before the first frame update
    [SyncVar]
    private int Day;
    [SyncVar]
    private int oldScore;
    [SyncVar]
    private int score;
    [SyncVar]
    private int catRecued;
    [SyncVar]
    private int catDied;
    [SyncVar]
    private int scoreTime;
    [SyncVar]
    private int scoreCatRescued;
    [SyncVar]
    private int scoreCatDied;
    public int TimeScorePerSecond;
    public int CatRecuedScore;
    public int CatDiedMinusScore;
    public static GameObject Singleton;
    private Timer Timer;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Day = 20;
        score = 0;
        catRecued = 0;
        catDied = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Score.Singleton == null) Score.Singleton = gameObject;
        else if (Score.Singleton != gameObject) Destroy(gameObject);
    }

    public int getDay()
    {
        return Day;
    }

    public void setDay(int d)
    {
        Day = d;
    }
    [ServerCallback]
    public void addDay(int d = 1)
    {
        Day = Day + d;
    }
    [ServerCallback]
    public void CalculateScore()
    {
        oldScore = score;
        Timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        scoreTime = Timer.getTimeLeft() * TimeScorePerSecond;
        scoreCatRescued = catRecued * CatRecuedScore;
        scoreCatDied = catDied * (- CatDiedMinusScore);
        score = oldScore + scoreTime + scoreCatRescued + scoreCatDied;
    }

    public int getScore()
    {
        return score;
    }

    public int getOldScore()
    {
        return oldScore;
    }

    public void setOldScoreToScore()
    {
        oldScore = score;
    }

    public int getScoreTime()
    {
        return scoreTime;
    }

    public int getScoreCatRescued()
    {
        return scoreCatRescued;
    }
    public int getScoreCatDied()
    {
        return scoreCatDied;
    }
    [ServerCallback]
    public void ResetScore()
    {
        score = 0;
    }
    [ServerCallback]
    public void ResetCat()
    {
        catRecued = 0;
        catDied = 0;
    }
    [ServerCallback]
    public void AddCatRecued()
    {
        catRecued++;
    }
    [ServerCallback]
    public void AddCatDied()
    {
        catDied++;
    }
}
