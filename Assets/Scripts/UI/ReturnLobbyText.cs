using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnLobbyText : MonoBehaviour
{
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    public void ChangeToRed()
    {
        text.color = Color.red;
    }

    public void ChangeToWhite()
    {
        text.color = Color.white;
    }
}
