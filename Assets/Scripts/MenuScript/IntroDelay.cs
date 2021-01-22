using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroDelay : MonoBehaviour
{

    public int Countdown;
    private int count;
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(count >= Countdown)
        {
            this.gameObject.SetActive(false);
        }
        count += 1;
    }
}
