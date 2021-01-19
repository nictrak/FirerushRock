using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIntro : MonoBehaviour
{
    // Start is called before the first frame update
    public int time;
    private int count;
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        count++;
        FireSystem.setRun(false);
        if (count >= time)
        {
            FireSystem.setRun(true);
            Destroy(this.gameObject);
        }
    }
}
