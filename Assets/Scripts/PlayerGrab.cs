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
    public float GrabScale;
    private Vector3 memGrabScale;
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
    public bool Grab(Grabbable grabed)
    {
        if (grabed != null && grabedObject == null)
        {
            if (grabed.IsGrabbable)
            {
                grabedObject = grabed;
                grabedObject.Grabed(this);
                return true;
            }
        }
        return false;
    }
    private void Release()
    {
        if (grabedObject != null)
        {
            grabedObject.Released(false);
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
                grabedObject.Released(true);
                throwed.IsThrowed = true;
                throwed.Thrower = this;
                if (playerDirection.Direction == "left")
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
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (grabedObject == null)
            {
                Grabbable grabed = usedGrab.CalNearest();
                Grab(grabed);
            }
            else
            {
                Release();
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (grabedObject != null)
            {
                Throw();
            }
        }
    }
}
