using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreText : MonoBehaviour
{
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        updateScore();
    }
    public void updateScore()
    {
        text = GetComponent<Text>();
        GameObject score = GameObject.FindGameObjectWithTag("Score");
        if(score != null)
        {
            this.text.text = "" + score.GetComponent<Score>().getScore();
        }
    }
}
