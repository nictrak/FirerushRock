﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    public static AudioClip Music;
    public static int Day;
    public static int ContinueDay;
    // Start is called before the first frame update
    void Start()
    {
        if (Day == 0)
        {
            Day = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
