using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update
    private int Day;
    private int oldScore;
    private int score;
    private int catRecued;
    private int catDied;
    private int scoreTime;
    private int scoreCatRescued;
    private int scoreCatDied;
    public int TimeScorePerSecond;
    public int CatRecuedScore;
    public int CatDiedMinusScore;

    private Timer Timer;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Day = 1;
        score = 0;
        catRecued = 0;
        catDied = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getDay()
    {
        return Day;
    }

    public void setDay(int d)
    {
        Day = d;
    }

    public void addDay(int d = 1)
    {
        Day = Day + d;
    }

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
    public void ResetScore()
    {
        score = 0;
    }

    public void ResetCat()
    {
        catRecued = 0;
        catDied = 0;
    }

    public void AddCatRecued()
    {
        catRecued++;
    }

    public void AddCatDied()
    {
        catDied++;
    }
}
