using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class scoreEnable : MonoBehaviour
{
    // Start is called before the first frame update
    private Score Score;
    public ScoreText UIScoreText;
    void Start()
    {
        DoOnStart();
    }
    private void DoOnStart()
    {
        Score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
        UIScoreText.updateScore();
    }
    // Update is called once per frame

    void Update()
    {
    }
}
