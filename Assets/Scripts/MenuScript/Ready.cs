using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class Ready : NetworkBehaviour
{
    public Text text;
    private bool isReady;
    public NetworkManager manager;

    [SyncVar]
    private int AlreadyReady;
    // Start is called before the first frame update
    void Start()
    {
        isReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ServerCallback]
    private void LateUpdate()
    {
        if(AlreadyReady == NetworkServer.connections.Count)
        {
            manager.ServerChangeScene("PlayScene");
        }
    }
    public void ToggleReady()
    {
        isReady = !isReady;
        if (isReady) text.text = "Not Ready";
        else text.text = "Ready";
        CmdReady(isReady);
    }
    [Command(ignoreAuthority = true)]
    private void CmdReady(bool isReady)
    {
        if (isReady)
        {
            AlreadyReady += 1;
        }
        else
        {
            AlreadyReady -= 1;
        }
    }
}
