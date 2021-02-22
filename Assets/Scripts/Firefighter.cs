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
    private int spawnCounter;

    private string lastScene;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        DontDestroyOnLoad(gameObject);
        lastScene = "";
        spawnCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        SpawnWhenSceneChange();
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
            transform.position = spawnPoint.transform.position;
            return true;
        }
        else return false;
    }
    public void Respawn()
    {
        transform.position = spawnPoint;
    }

    private void SpawnWhenSceneChange()
    {
        string activeNow = SceneManager.GetActiveScene().name;
        if (lastScene == "") lastScene = activeNow;
        if(lastScene != activeNow)
        {
            if (spawnCounter >= 30)
            {
                if (ToSpawnPoint())
                {
                    lastScene = activeNow;
                    spawnCounter = 0;
                }
            }
            else spawnCounter += 1;
        }
    }
}
