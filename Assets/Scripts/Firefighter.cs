using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class Firefighter : NetworkBehaviour
{
    private Rigidbody2D rigidbody;

    public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }

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
        if(SceneManager.GetActiveScene().name == "PlayScene")
        {
            if (isLocalPlayer) GetComponent<GenerateCamera>().PlayerCamera.gameObject.SetActive(true);
            GetComponent<PlayerControl>().isEnable = true;
            GetComponent<PlayerDirection>().IsEnable = true;
        }
    }
}
