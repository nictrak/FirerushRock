using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class Firefighter : NetworkBehaviour
{
    private Rigidbody2D rigidbody;

    public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }

    private Vector3 spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        /*if(SceneManager.GetActiveScene().name == "PlayScene")
        {
            
        }*/
        if(spawnPoint != null && isLocalPlayer)
        Debug.Log(spawnPoint);
    }
    [ClientRpc]
    public void ToOrigin()
    {
        Debug.Log("Rpc to origin");
        transform.position = new Vector3();
    }
    [ClientRpc]
    public void SetSpawnPoint(Vector3 point)
    {
        spawnPoint = point;
    }
    [ClientRpc]
    public void ToSpawnPoint()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        transform.position = spawnPoint.transform.position;
    }
    public void Respawn()
    {
        transform.position = spawnPoint;
    }
}
