﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    private bool isThrowed;
    private Vector2 throwVector;
    private Rigidbody2D rigidbody;
    private BoxCollider2D collider;
    public bool IsThrowed { get => isThrowed; set => isThrowed = value; }
    public Vector2 ThrowVector { get => throwVector; set => throwVector = value; }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
           
    }
    private void FixedUpdate()
    {
        ThrowedMove();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (throwVector.magnitude > 0.00001)
        {
            isThrowed = false;
            throwVector = new Vector2();
            collider.isTrigger = true;
            
        }
    }
    private void ThrowedMove()
    {
        if (isThrowed)
        {
            rigidbody.position += throwVector;
        }
    }
}