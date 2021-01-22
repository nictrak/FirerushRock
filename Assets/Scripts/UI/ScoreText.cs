using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    private Text text;
    private Score Score;
    // Start is called before the first frame update
    void Start()
    {
        Score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
        //Debug.Log(Score.getScore());
        text = GetComponent<Text>();
        this.text.text = "" + Score.getScore();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void updateScore()
    {
        Score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
        //Debug.Log(Score.getScore());
        text = GetComponent<Text>();
        this.text.text = "" + Score.getScore();
    }
}
