using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SetupSystem : NetworkBehaviour
{
    private GameObject[] Firefighters;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [ServerCallback]
    private void GetAllFirefighter()
    {
        Firefighters = GameObject.FindGameObjectsWithTag("Player");
    }
    [ServerCallback]
    public void FirefighterToSpawnPoint(Vector3 point)
    {
        GetAllFirefighter();
        for (int i = 0; i < Firefighters.Length; i++)
        {
            Firefighters[i].GetComponent<Firefighter>().SetSpawnPoint(point);
            Firefighters[i].GetComponent<Firefighter>().ToSpawnPoint();
        }
    }
}
