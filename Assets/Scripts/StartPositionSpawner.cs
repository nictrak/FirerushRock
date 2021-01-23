using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class StartPositionSpawner : NetworkBehaviour
{
    public GameObject StartPosition;
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }
    [ServerCallback]
    private void Spawn()
    {
        GameObject obj = Instantiate(StartPosition);
        obj.transform.position = transform.position;
        NetworkServer.Spawn(obj);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
