﻿using System.Collections;
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

    private bool isMove;

    public float moveVelocityHorizontal;
    public float moveVelocityVertical;
    public bool isEnable;

    public bool IsMove { get => isMove; set => isMove = value; }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        leftVector = new Vector2(-moveVelocityHorizontal, 0);
        rightVector = new Vector2(moveVelocityHorizontal, 0);
        upVector = new Vector2(0, moveVelocityVertical);
        downVector = new Vector2(0, -moveVelocityVertical);
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
        Move();
    }
    private void Move()
    {
        if (isEnable)
        {
            rigidbody.MovePosition(rigidbody.position + moveVector);
            if (moveVector.magnitude > 0.0001)
            {
                isMove = true;
            }
            else
            {
                isMove = false;
            }

        }
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

    /*private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<BreakWater>() != null)
        {
            Vector3 collisionPosition = collision.gameObject.transform.position;

            if (transform.position.x > collisionPosition.x)
            {
                Debug.Log("Bounce right");
                rigidbody.MovePosition(rigidbody.position + 10 * rightVector);
            }
            else
            {
                Debug.Log("Bounce left");
                rigidbody.MovePosition(rigidbody.position + 10 * leftVector);
            }
            if (transform.position.y > collisionPosition.y)
            {
                Debug.Log("Bounce up");
                rigidbody.MovePosition(rigidbody.position + 10 * upVector);
            }
            else
            {
                Debug.Log("Bounce down");
                rigidbody.MovePosition(rigidbody.position + 10 * downVector);
            }
        }
    }*/

}
