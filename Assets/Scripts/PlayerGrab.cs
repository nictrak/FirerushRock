﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerGrab : NetworkBehaviour
{
    private GrabHitbox usedGrab;
    private Grabbable grabedObject;
    private PlayerDirection playerDirection;
    private AudioSource audioSource;

    public GrabHitbox LeftGrab;
    public GrabHitbox RightGrab;
    public GrabHitbox UpGrab;
    public GrabHitbox DownGrab;
    public float ThrowSpeed;
    public float GrabScale;
    public float ReleaseLenght;
    public AudioClip GrabSound;
    public AudioClip ReleaseSound;
    public AudioClip ThrowSound;
    private Vector3 memGrabScale;
    public GrabHitbox UsedGrab { get => usedGrab; set => usedGrab = value; }
    public Grabbable GrabedObject { get => grabedObject; set => grabedObject = value; }

    // Start is called before the first frame update
    void Start()
    {
        playerDirection = GetComponent<PlayerDirection>();
        usedGrab = LeftGrab;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameConfig.SfxVolume;
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
    public void PositionFixedRelease(Vector3 position)
    {
        if (grabedObject != null)
        {
            Grabbable temp = grabedObject;
            Vector3 releasedVector = CalReleasedVector();
            CmdRelease(grabedObject.netIdentity, false, releasedVector);
            grabedObject = null;
            temp.transform.position = position;
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
            Survivor cat = grabedIdentity.GetComponent<Survivor>();
            Grabbable grabed = grabedIdentity.GetComponent<Grabbable>();
            Breakable breakable = grabedIdentity.GetComponent<Breakable>();
            if (grabed != null) grabed.Released(newIsTrigger, releasedVector);
            if (breakable != null)
            {
                breakable.IsEnable = true;
            }
            if (cat != null)
            {
                cat.RpcPlaySfx();
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
            throwed.Unthrowed(true);
        }
    }
    private void GrabControl()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Z))
        {
            if (grabedObject == null)
            {
                Grabbable grabed = usedGrab.CalNearest();
                if (Grab(grabed))
                    audioSource.PlayOneShot(GrabSound);
            }
            else
            {
                Release();
                audioSource.PlayOneShot(ReleaseSound);
            }
        }
        if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.C))
        {
            if (grabedObject != null)
            {
                Throw();
                audioSource.PlayOneShot(ThrowSound);
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
    public bool IsGrab()
    {
        if(grabedObject != null)
        {
            return true;
        }
        return false;
    }
    private bool IsReleaseBreak()
    { 
        if(usedGrab.BreakWaters.Count == 0)
        {
            return false;
        }else if(usedGrab.BreakWaters.Count == 1)
        {
            if(usedGrab.BreakWaters[0].GetComponent<Grabbable>() == GrabedObject)
            {
                return false;
            }
        }
        return true;
    }
    private void KnockBackBeforeRelease(Vector3 releasedVector)
    {
        string direction = playerDirection.Direction;
        Vector3 AdditionalKnock = new Vector3();
        if (direction == "left")
        {
            AdditionalKnock = new Vector3(GrabedObject.Width/2 - 0.5f, 0, 0);
        }
        else if (direction == "right")
        {
            AdditionalKnock = new Vector3(-GrabedObject.Width/2 + 0.5f, 0, 0);
        }
        else if (direction == "up")
        {
            AdditionalKnock = new Vector3(0, -GrabedObject.Width/2 + 0.5f, 0);
        }
        else if (direction == "down")
        {
            AdditionalKnock = new Vector3(0, GrabedObject.Width/2 - 0.5f, 0);
        }
        transform.position = transform.position - releasedVector + AdditionalKnock;
    }
}
