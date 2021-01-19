using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    // Start is called before the first frame update
    private int count;
    public int time;
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time++;
        if(time >= count)
        {
            SceneManager.LoadScene("testPrint");
        }
    }
}
