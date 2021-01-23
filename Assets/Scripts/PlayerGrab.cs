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
        else if (direction == "right")
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
        Debug.Log("Grab");
        Debug.Log(grabed);
        if (grabed != null && grabedObject == null)
        {
            if (grabed.IsGrabbable)
            {
                if (grabed.IsGrabed) CmdRelease(grabed.netIdentity, true, new Vector3());
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
        if(grabberIdentity != null && grabedIdentity != null)
        {
            Grabbable grabed = grabedIdentity.GetComponent<Grabbable>();
            Breakable breakable = grabedIdentity.GetComponent<Breakable>();
            if (grabed != null) grabed.Grabed(grabberIdentity);
            if (breakable != null)
            {
                breakable.IsEnable = false;
            }
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
    [ClientRpc]
    public void ForceRelease()
    {
        if (grabedObject != null)
        {
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
        if(grabedIdentity != null && releasedVector != null)
        {
            Grabbable grabed = grabedIdentity.GetComponent<Grabbable>();
            Breakable breakable = grabedIdentity.GetComponent<Breakable>();
            if (grabed != null) grabed.Released(newIsTrigger, releasedVector);
            if (breakable != null)
            {
                breakable.IsEnable = true;
            }
        }
    }
    private void Throw()
    {
        if(grabedObject != null)
        {
            if (grabedObject.GetComponent<Throwable>() != null)
            {
                CmdRelease(grabedObject.netIdentity, true, new Vector3());
                CmdThrow(netIdentity, grabedObject.netIdentity, playerDirection.Direction, ThrowSpeed);
                grabedObject = null;
            }
        }
    }
    [Command]
    private void CmdThrow(NetworkIdentity throwerIdentity, NetworkIdentity throwedIdentity, string direction, float throwSpeed)
    {
        if(throwerIdentity != null && throwedIdentity != null)
        {
            Throwable throwed = throwedIdentity.GetComponent<Throwable>();
            throwed.Throwed(throwerIdentity, direction, throwSpeed);
        }
    }
    [Command]
    private void CmdUnthrow(NetworkIdentity throwedIdentity)
    {
        if (throwedIdentity != null)
        {
            Throwable throwed = throwedIdentity.GetComponent<Throwable>();
            throwed.Unthrowed();
        }
    }
    private void GrabControl()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Z))
        {
            if (grabedObject == null)
            {
                Debug.Log(playerDirection.Direction);
                SelectGrab();
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isLocalPlayer)
        {
            Grabbable incomingGrabbable = collision.gameObject.GetComponent<Grabbable>();
            Throwable incomingThrowable = collision.gameObject.GetComponent<Throwable>();
            if (incomingGrabbable != null && incomingThrowable != null)
            {
                if (incomingThrowable.IsThrowed && incomingThrowable.Thrower != netIdentity)
                {
                    CmdUnthrow(incomingThrowable.netIdentity);
                    Grab(incomingGrabbable);
                }
            }
        }
    }
}
