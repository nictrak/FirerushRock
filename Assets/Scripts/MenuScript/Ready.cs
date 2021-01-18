using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Ready : NetworkBehaviour
{
    public Text text;
    private bool isReady;
    // Start is called before the first frame update
    void Start()
    {
        isReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleReady()
    {
        isReady = !isReady;
        if (isReady) text.text = "Not Ready";
        else text.text = "Ready";
    }
}
