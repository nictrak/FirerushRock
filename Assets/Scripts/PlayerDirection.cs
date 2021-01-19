using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerDirection : NetworkBehaviour
{
    private string direction;
    private string startDirection;
    public bool IsEnable;

    public string Direction { get => direction; set => direction = value; }
    // Start is called before the first frame update
    void Start()
    {
        direction = "down";
        startDirection = direction;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEnable) {
            if (isLocalPlayer) SelectDirection();
        }
        else
        {
            direction = startDirection;
        }
    }

    private void SelectDirection()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Direction = "left";
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Direction = "right";
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Direction = "up";
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Direction = "down";
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Direction = "left";
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Direction = "right";
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            Direction = "up";
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Direction = "down";
        }
    }
}
