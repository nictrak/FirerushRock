using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WinScoreText : MonoBehaviour
{
    // Start is called before the first frame update
    private Text text;
    private GameObject scoreObj;
    private Score score;
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreObj = GameObject.FindGameObjectWithTag("Score");
        if(scoreObj != null)
        {
            score = scoreObj.GetComponent<Score>();
            updateWinScore(score.getOldScore(), score.getScoreTime(), score.getScoreCatRescued(), score.getScoreCatDied(), score.getScore());
        }
    }
    public void updateWinScore(int oldScore,int TimeScore, int CatRes, int CatDied, int Score)
    {
        text = GetComponent<Text>();
        text.text = "" + oldScore + "\n" + TimeScore + "\n" + CatRes + "\n" + CatDied + "\n" + Score;
    }
}
