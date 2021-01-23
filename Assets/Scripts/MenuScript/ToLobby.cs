using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class ToLobby : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToLobbyCommand()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerGrab>().ForceRelease();
        }
        GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkManager>().ServerChangeScene("PlayScene");
    }
}
