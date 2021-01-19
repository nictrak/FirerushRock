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
    private void ActivateAllFirefighter()
    {
        for(int i = 0; i < Firefighters.Length; i++)
        {
            Debug.Log(i);
        }
    }
}
