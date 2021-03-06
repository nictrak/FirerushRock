﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Dispenser : NetworkBehaviour
{
    public GameObject DispensedPrefab;

    private GameObject aliveObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DispenseLoop();
        SyncAlive();
    }
    [ServerCallback]
    private void DispenseLoop()
    {
        if(aliveObject == null)
        {
            aliveObject = Instantiate<GameObject>(DispensedPrefab);
            aliveObject.transform.position = transform.position;
            NetworkServer.Spawn(aliveObject);
        }
    }
    private void SyncAlive()
    {
        if(aliveObject != null)
        {
            if (!CheckSamePosition2D(aliveObject))
            {
                aliveObject = null;
            }
        }
    }
    private bool CheckSamePosition2D(GameObject obj)
    {
        Vector3 objPosition = obj.transform.position;
        if (objPosition.x == transform.position.x && objPosition.y == transform.position.y)
        {
            return true;
        }
        return false;
    }
}
