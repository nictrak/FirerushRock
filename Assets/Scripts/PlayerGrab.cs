using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    private GrabHitbox usedGrab;
    private Grabbable grabedObject;
    private PlayerDirection playerDirection;

    public GrabHitbox LeftGrab;
    public GrabHitbox RightGrab;
    public GrabHitbox UpGrab;
    public GrabHitbox DownGrab;
    public float ThrowSpeed;
    
    public GrabHitbox UsedGrab { get => usedGrab; set => usedGrab = value; }
    public Grabbable GrabedObject { get => grabedObject; set => grabedObject = value; }

    // Start is called before the first frame update
    void Start()
    {
        playerDirection = GetComponent<PlayerDirection>(); 
        usedGrab = LeftGrab;
    }

    // Update is called once per frame
    void Update()
    {
        SelectGrab();
        GrabControl();
    }

    

    public void UseObject()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GrabedObject.Use();
        }
    }
    
    private void SelectGrab()
    {
        string direction = playerDirection.Direction;
        if(direction == "left")
        {
            usedGrab = LeftGrab;
        }
        else if (direction == "rigth")
        {
            usedGrab = RightGrab;
        }
        else if (direction == "up")
        {
            usedGrab = UpGrab;
        }
        else if (direction == "down")
        {
            usedGrab = DownGrab;
        }
    }
    private void Grab()
    {
        Grabbable grabed = usedGrab.CalNearest();
        Rigidbody2D grabedRigidbody;
        if (grabed != null)
        {
            grabedObject = grabed;
            grabedObject.IsGrabed = true;
            grabedObject.Grabber = this;
            grabedObject.transform.position = transform.position;
            grabedObject.GetComponent<ZSync>().LavitateHeight = GetComponent<ZSync>().SpriteHeight / 2 + 0.2f;
            grabedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            grabedObject.transform.position = transform.position;
        }
    }
    private void Release()
    {
        float releasePositionY = transform.position.y - GetComponent<ZSync>().SpriteHeight / 2 + grabedObject.GetComponent<ZSync>().SpriteHeight / 2 - 0.0001f;
        if (grabedObject != null)
        {
            grabedObject.IsGrabed = false;
            grabedObject.Grabber = null;
            grabedObject.transform.position = new Vector2(transform.position.x ,releasePositionY);
            grabedObject.GetComponent<ZSync>().LavitateHeight = 0;
            grabedObject = null;
        }
    }
    private void Throw()
    {
        if(grabedObject != null)
        {
            Throwable throwed = grabedObject.GetComponent<Throwable>();
            Vector2 throwVector;
            if (grabedObject.GetComponent<Throwable>() != null)
            {
                grabedObject.IsGrabed = false;
                grabedObject.Grabber = null;
                grabedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                //grabedObject.GetComponent<BoxCollider2D>().isTrigger = false;
                throwed.IsThrowed = true;
                if(playerDirection.Direction == "left")
                {
                    throwVector = new Vector2(-ThrowSpeed, 0);
                }
                else if (playerDirection.Direction == "right")
                {
                    throwVector = new Vector2(ThrowSpeed, 0);
                }
                else if (playerDirection.Direction == "up")
                {
                    throwVector = new Vector2(0, ThrowSpeed);
                }
                else
                {
                    throwVector = new Vector2(0, -ThrowSpeed);
                }
                throwed.ThrowVector = throwVector;
                grabedObject = null;
            }
        }
    }
    private void GrabControl()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (grabedObject == null)
            {
                Grab();
            }
            else
            {
                Release();
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (grabedObject != null)
            {
                Throw();
            }
        }
    }
}
