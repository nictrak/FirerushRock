using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Throwable : NetworkBehaviour
{
    private bool isThrowed;
    private Vector2 throwVector;
    private Rigidbody2D rigidbody;
    private BoxCollider2D collider;
    private bool isBreakActive;
    private NetworkIdentity thrower;
    public bool IsThrowed { get => isThrowed; set => isThrowed = value; }
    public Vector2 ThrowVector { get => throwVector; set => throwVector = value; }
    public bool IsBreakActive { get => isBreakActive; set => isBreakActive = value; }

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
        SyncIsBreak();
        ThrowedMove();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*BreakThrow breakThrow = collision.gameObject.GetComponent<BreakThrow>();
        PlayerGrab playerGrab = collision.gameObject.GetComponent<PlayerGrab>();
        if (breakThrow != null && throwVector.magnitude > 0.00001)
        {
            isThrowed = false;
            throwVector = new Vector2();
            collider.isTrigger = false;
        }
        if(playerGrab != null)
        {
            if(playerGrab != thrower)
            if (playerGrab.Grab(this.GetComponent<Grabbable>()))
            {
                isThrowed = false;
                throwVector = new Vector2();
            }
        }*/
    }
    [ServerCallback]
    private void SyncIsBreak()
    {
        if (throwVector.magnitude > 0.00001)
        {
            isBreakActive = true;
        }
        else
        {
            isBreakActive = false;
        }
    }
    [ServerCallback]
    private void ThrowedMove()
    {
        if (isThrowed)
        {
            rigidbody.position += throwVector;
        }
    }
    public void Throwed(NetworkIdentity throwerIdentity, string direction, float throwSpeed)
    {
        Vector2 throwVector;
        isThrowed = true;
        thrower = throwerIdentity;
        if (direction == "left")
        {
            throwVector = new Vector2(-throwSpeed, 0);
        }
        else if (direction == "right")
        {
            throwVector = new Vector2(throwSpeed, 0);
        }
        else if (direction == "up")
        {
            throwVector = new Vector2(0, throwSpeed);
        }
        else
        {
            throwVector = new Vector2(0, -throwSpeed);
        }
        this.throwVector = throwVector;
    }
}
