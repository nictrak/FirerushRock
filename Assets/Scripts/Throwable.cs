using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    private bool isThrowed;
    private Vector2 throwVector;
    private Rigidbody2D rigidbody;
    private BoxCollider2D collider;
    private bool isBreakActive;
    private PlayerGrab thrower;
    public bool IsThrowed { get => isThrowed; set => isThrowed = value; }
    public Vector2 ThrowVector { get => throwVector; set => throwVector = value; }
    public bool IsBreakActive { get => isBreakActive; set => isBreakActive = value; }
    public PlayerGrab Thrower { get => thrower; set => thrower = value; }

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
        BreakThrow breakThrow = collision.gameObject.GetComponent<BreakThrow>();
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
        }
    }
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
    private void ThrowedMove()
    {
        if (isThrowed)
        {
            rigidbody.position += throwVector;
        }
    }
}
