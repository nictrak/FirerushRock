using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatIntro : MonoBehaviour
{
    // Start is called before the first frame update
    private Text text;
    private bool done;
    void Start()
    {
        done = false;
        text = this.GetComponent<Text>();
        text.color = Color.black;
        text.text = "Cat : " + LevelScriptController.Cat;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (done == false) {
            if (LevelScriptController.Cat >= 0)
             {
                text.text = "Cat : " + LevelScriptController.Cat;
                text.color = Color.white;
                done = true;
             }
        } 
    }
}
