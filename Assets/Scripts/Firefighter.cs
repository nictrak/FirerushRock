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

    private bool isLoadNewLevel;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject);
        isLoadNewLevel = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        ToSpawnPointLoop();
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
    public bool ToSpawnPoint()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        if (spawnPoint != null)
        {
            rigidbody.position = new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.y);
            return true;
        }
        else return false;
    }
    public void Respawn()
    {
        transform.position = spawnPoint;
    }
    private void OnLevelWasLoaded(int level)
    {
        isLoadNewLevel = true;
    }
    private void ToSpawnPointLoop()
    {
        if (isLoadNewLevel)
        {
            if (ToSpawnPoint())
            {
                isLoadNewLevel = false;
            }
        }
    }
}
