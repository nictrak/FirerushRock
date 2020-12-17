using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private Vector2 leftVector;
    private Vector2 rightVector;
    private Vector2 upVector;
    private Vector2 downVector;
    private Vector2 idleVector;
    private Vector2 moveVector;

    public float moveVelocity;
    public bool isEnable;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        leftVector = new Vector2(-moveVelocity, 0);
        rightVector = new Vector2(moveVelocity, 0);
        upVector = new Vector2(0, moveVelocity);
        downVector = new Vector2(0, -moveVelocity);
        idleVector = new Vector2();
        moveVector = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = MoveControl();
    }
    private void FixedUpdate()
    {
        if (isEnable) rigidbody.MovePosition(rigidbody.position + moveVector);
    }
    private Vector2 MoveControl()
    {
        Vector2 outVector = idleVector;
        if (Input.GetKey(KeyCode.A))
        {
            outVector += leftVector;
        }
        if (Input.GetKey(KeyCode.D))
        {
            outVector += rightVector;
        }
        if (Input.GetKey(KeyCode.W))
        {
            outVector += upVector;
        }
        if (Input.GetKey(KeyCode.S))
        {
            outVector += downVector;
        }
        return outVector;
    }
}
