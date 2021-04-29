using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
public class Grabbable : NetworkBehaviour
{
    [SyncVar]
    private bool isGrabed;
    [SyncVar]
    [SerializeField]
    private bool isGrabbable;
    [SyncVar]
    private NetworkIdentity grabber;
    [SyncVar]
    private Vector3 serverScale;
    private int syncCounter;
    private Rigidbody2D rigidbody;
    private ZSync zSync;
    private BoxCollider2D collider2D;
    public float Height;
    public float Width;
    private string lastScene;
    public float SpeedMultiplier;
    public int SyncServerTime;
    public bool IsGrabed { get => isGrabed; set => isGrabed = value; }
    public NetworkIdentity Grabber { get => grabber; set => grabber = value; }
    public ZSync ZSync { get => zSync; set => zSync = value; }
    public BoxCollider2D Collider2D { get => collider2D; set => collider2D = value; }
    public bool IsGrabbable { get => isGrabbable; set => isGrabbable = value; }
    public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }

    public Vector3 GrabedPostion;

    // Start is called before the first frame update
    void Start()
    {
        isGrabed = false;
        rigidbody = GetComponent<Rigidbody2D>();
        zSync = GetComponent<ZSync>();
        collider2D = GetComponent<BoxCollider2D>();
        isGrabbable = true;
        lastScene = SceneManager.GetActiveScene().name;
        syncCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        string newScene = SceneManager.GetActiveScene().name;
        if(newScene != lastScene)
        {
            Destroy(gameObject);
        }
    }
    [ServerCallback]
    private void ServerSync()
    {
        serverScale = transform.localScale;
    }
    [ClientCallback]
    private void ClientSync()
    {
        if(grabber != null)
        {
            if(transform.parent == null)
            {
                collider2D.isTrigger = true;
                transform.parent = grabber.transform;
                zSync.IsEnable = false;
                transform.localPosition = GrabedPostion;
                transform.localScale = serverScale;
            }
        }
        //transform.localScale = serverScale;
    }
    private void FixedUpdate()
    {
        if (syncCounter >= SyncServerTime)
        {
            ServerSync();
            ClientSync();
        }
        else syncCounter += 1;
        //MoveWhenGrabed();
    }
    public void Use()
    {

    }
    [ClientRpc]
    public void Grabed(NetworkIdentity grabber)
    {
        if(grabber != null)
        {
            rigidbody = GetComponent<Rigidbody2D>();
            zSync = GetComponent<ZSync>();
            collider2D = GetComponent<BoxCollider2D>();
            IsGrabed = true;
            this.grabber = grabber;
            collider2D.isTrigger = true;
            transform.parent = grabber.transform;
            zSync.IsEnable = false;
            transform.localPosition = GrabedPostion;
        }
    }
    [ClientRpc]
    public void Released(bool newIsTrigger, Vector3 releasedVector)
    {
        if(releasedVector != null && grabber != null)
        {
            rigidbody = GetComponent<Rigidbody2D>();
            zSync = GetComponent<ZSync>();
            collider2D = GetComponent<BoxCollider2D>();
            IsGrabed = false;
            Vector3 translation = new Vector3(-Width / 2 + 0.5f, Height / 2 - 0.5f, 0);
            transform.position = grabber.transform.position + releasedVector + translation;
            collider2D.isTrigger = newIsTrigger;
            transform.parent = null;
            zSync.IsEnable = true;
            this.grabber = null; 
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
        }
    }
    private void MoveWhenGrabed()
    {
        if (isGrabed)
        {
            rigidbody.position = grabber.GetComponent<Rigidbody2D>().position;
        }
    }
}
