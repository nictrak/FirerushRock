using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    public ScoreText ScoreText;
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Lose()
    {
        gameObject.SetActive(true);
        ScoreText.updateScore();
    }  
}
