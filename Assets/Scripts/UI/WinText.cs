using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WinText : MonoBehaviour
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
        if (scoreObj != null)
        {
            score = scoreObj.GetComponent<Score>();
            updateWinScore(score.getScoreCatRescued(), score.getScoreCatDied());
        }
    }
    public void updateWinScore(int CatRes, int CatDied)
    {
        text = GetComponent<Text>();
        if (CatRes == 0 && CatDied == 0)
        {
            text.text = "Current Score :" + "\n" + "Time Left :" + "\n" + "New Score :";
        }
        if (CatRes != 0 && CatDied != 0)
        {
            text.text = "Current Score :" + "\n" + "Time Left :" + "\n" + "Cat Rescued :" + "\n" + "Cat go Isekai:" + "\n" + "New Score :";
        }
        if (CatRes != 0 && CatDied == 0)
        {
            text.text = "Current Score :" + "\n" + "Time Left :" + "\n" + "Cat Rescued :" + "\n" + "New Score :";
        }
        if (CatRes == 0 && CatDied != 0)
        {
            text.text = "Current Score :" + "\n" + "Time Left :" + "\n" + "Cat go Isekai:" + "\n" + "New Score :";
        }

        //text.text = "Current Score :" + "\n" + "Time Left :" + "\n" + "Cat Rescued :" + "\n" + "Cat go Isekai:" + "\n" + "New Score :";
    }
}
