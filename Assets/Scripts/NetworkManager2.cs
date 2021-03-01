using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class NetworkManager2 : NetworkManager
{
    public GameObject PlayerPrefab1;
    public GameObject PlayerPrefab2;
    public GameObject PlayerPrefab3;
    public GameObject PlayerPrefab4;

    private bool isUse1;
    private bool isUse2;
    private bool isUse3;
    private bool isUse4;

    private int player1;
    private int player2;
    private int player3;
    private int player4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject gameobject;
        gameobject = SpawnOnStart(SelectPrefab(conn));
        NetworkServer.AddPlayerForConnection(conn, gameobject);
    }
    private GameObject SpawnOnStart(GameObject prefab)
    {
        Transform startPos = GetStartPosition();
        GameObject obj = startPos != null
                ? Instantiate(prefab, startPos.position, startPos.rotation)
                : Instantiate(prefab);
        return obj;
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (conn.connectionId == player1)
        {
            isUse1 = false;
        }
        else if (conn.connectionId == player2)
        {
            isUse2 = false;
        }
        else if (conn.connectionId == player3)
        {
            isUse3 = false;
        }
        else if (conn.connectionId == player4)
        {
            isUse4 = false;
        }
        base.OnServerDisconnect(conn);
    }
    private GameObject SelectPrefab(NetworkConnection conn)
    {
        GameObject obj = null;
        if (!isUse1)
        {
            obj = PlayerPrefab1;
            isUse1 = true;
            player1 = conn.connectionId;
        }
        else if (!isUse2)
        {
            obj = PlayerPrefab2;
            isUse2 = true;
            player2 = conn.connectionId;
        }
        else if (!isUse3)
        {
            obj = PlayerPrefab3;
            isUse3 = true;
            player3 = conn.connectionId;
        }
        else if (!isUse4)
        {
            obj = PlayerPrefab4;
            isUse4 = true;
            player4 = conn.connectionId;
        }
        else obj = PlayerPrefab1;
        return obj;
    }
    public override void OnStopHost()
    {
        base.OnStopHost();
        Destroy(GameObject.FindGameObjectWithTag("Score"));
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
        Destroy(GameObject.FindGameObjectWithTag("Score"));
    }

    public int CountPlayer()
    {
        int i = 0;
        if (isUse1) i++;
        if (isUse2) i++;
        if (isUse3) i++;
        if (isUse4) i++;
        return i;

    }
}
