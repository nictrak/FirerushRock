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
    private PlayerGrab grabber;
    private Rigidbody2D rigidbody;
    private ZSync zSync;
    private BoxCollider2D collider2D;
    public float Height;
    public float Width;

    public float SpeedMultiplier;
    public bool IsGrabed { get => isGrabed; set => isGrabed = value; }
    public PlayerGrab Grabber { get => grabber; set => grabber = value; }
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
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {
        //MoveWhenGrabed();
    }
    public void Use()
    {

    }
    [ClientRpc]
    public void Grabed(PlayerGrab grabber)
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
    [ClientRpc]
    public void Released(bool newIsTrigger, Vector3 releasedVector)
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
    private void MoveWhenGrabed()
    {
        if (isGrabed)
        {
            rigidbody.position = grabber.GetComponent<Rigidbody2D>().position;
        }
    }
}
