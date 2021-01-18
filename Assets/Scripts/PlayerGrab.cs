using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerGrab : NetworkBehaviour
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
        if (isLocalPlayer)
        {
            SelectGrab();
            GrabControl();
        }
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
                CmdGrab(netIdentity, grabed.netIdentity);
                return true;
            }
        }
        return false;
    }
    [Command]
    private void CmdGrab(NetworkIdentity grabberIdentity, NetworkIdentity grabedIdentity)
    {
        PlayerGrab playerGrab = grabberIdentity.GetComponent<PlayerGrab>();
        Grabbable grabed = grabedIdentity.GetComponent<Grabbable>();
        Breakable breakable = grabedIdentity.GetComponent<Breakable>();
        grabed.Grabed(playerGrab);
        if(breakable != null)
        {
            breakable.IsEnable = false;
        }

    }
    private void Release()
    {
        if (grabedObject != null)
        {
            CmdRelease(grabedObject.netIdentity, false);
            grabedObject = null;
        }
    }
    [Command]
    private void CmdRelease(NetworkIdentity grabedIdentity, bool newIsTrigger)
    {
        Grabbable grabed = grabedIdentity.GetComponent<Grabbable>();
        Breakable breakable = grabedIdentity.GetComponent<Breakable>();
        grabed.Released(newIsTrigger);
        if (breakable != null)
        {
            breakable.IsEnable = false;
        }
    }
    private void Throw()
    {
        if(grabedObject != null)
        {
            if (grabedObject.GetComponent<Throwable>() != null)
            {
                CmdRelease(grabedObject.netIdentity, true);
                CmdThrow(netIdentity, grabedObject.netIdentity, playerDirection.Direction, ThrowSpeed);
                grabedObject = null;
            }
        }
    }
    [Command]
    private void CmdThrow(NetworkIdentity throwerIdentity, NetworkIdentity throwedIdentity, string direction, float throwSpeed)
    {
        Throwable throwed = throwedIdentity.GetComponent<Throwable>();
        throwed.Throwed(throwerIdentity, direction, throwSpeed);
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
