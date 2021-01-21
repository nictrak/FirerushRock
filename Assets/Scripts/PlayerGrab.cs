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
    public float ReleaseLenght;
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
        Debug.Log(grabed);
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
            Vector3 releasedVector = CalReleasedVector();
            CmdRelease(grabedObject.netIdentity, false, releasedVector);
            grabedObject = null;
        }
    }
    private Vector3 CalReleasedVector()
    {
        Vector3 releasedVector = new Vector3();
        if (playerDirection.Direction == "left")
        {
            releasedVector = new Vector3(-ReleaseLenght, 0, 0);
        }
        else if (playerDirection.Direction == "right")
        {
            releasedVector = new Vector3(ReleaseLenght, 0, 0);
        }
        else if (playerDirection.Direction == "up")
        {
            releasedVector = new Vector3(0, ReleaseLenght, 0);
        }
        else if (playerDirection.Direction == "down")
        {
            releasedVector = new Vector3(0, -ReleaseLenght, 0);
        }
        return releasedVector;
    }
    [Command]
    private void CmdRelease(NetworkIdentity grabedIdentity, bool newIsTrigger, Vector3 releasedVector)
    {
        Grabbable grabed = grabedIdentity.GetComponent<Grabbable>();
        Breakable breakable = grabedIdentity.GetComponent<Breakable>();
        grabed.Released(newIsTrigger, releasedVector);
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
                Vector3 releasedVector = CalReleasedVector();
                CmdRelease(grabedObject.netIdentity, true, releasedVector);
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
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Z))
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
        if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.C))
        {
            if (grabedObject != null)
            {
                Throw();
            }
        }
    }
}
