using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update
    private int Day;
    private int score;
    private int catRecued;
    private int catDied;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        score = 11000;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startf()
    {
        
    } 

    public int getScore()
    {
        return score;
    }

    public int getDay()
    {
        return Day;
    }
}
