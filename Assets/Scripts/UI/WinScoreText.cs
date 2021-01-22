using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScoreText : MonoBehaviour
{
    // Start is called before the first frame update
    private Text text;
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateWinScore(int oldScore,int TimeScore, int CatRes, int CatDied, int Score)
    {
        text.text = "" + oldScore + "\n" + TimeScore + "\n" + CatRes + "\n" + CatDied + "\n" + Score;
    }
}
