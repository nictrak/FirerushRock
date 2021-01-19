using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class SyncNetworkManager : MonoBehaviour
{
    private NetworkManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartHost()
    {
        manager.StartHost();
    }
    public void StartClient()
    {
        manager.StartClient();
    }
}
