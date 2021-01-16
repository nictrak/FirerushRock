using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerDirection : NetworkBehaviour
{
    private string direction;

    public string Direction { get => direction; set => direction = value; }
    // Start is called before the first frame update
    void Start()
    {
        direction = "left";
    }

    // Update is called once per frame
    void Update()
    {
        if(isLocalPlayer) SelectDirection();
    }

    private void SelectDirection()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Direction = "left";
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Direction = "right";
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Direction = "up";
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Direction = "down";
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Direction = "left";
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Direction = "right";
        }
        else if (Input.GetKey(KeyCode.W))
        {
            Direction = "up";
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Direction = "down";
        }
    }
}
