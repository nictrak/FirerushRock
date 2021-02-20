using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Breakable : NetworkBehaviour
{
    [SyncVar]
    private int toughness;
    [SyncVar]
    private bool isEnable;

    private Throwable throwable;

    public int MaxToughness;

    private GameObject bangPrefab;

    public bool IsEnable { get => isEnable; set => isEnable = value; }
    public int Toughness { get => toughness; set => toughness = value; }

    public bool IsDoor;

    public GameObject DoorHitSfx;
    public GameObject DoorBreakSfx;

    // Start is called before the first frame update
    void Start()
    {
        toughness = MaxToughness;
        isEnable = true;
        throwable = GetComponent<Throwable>();
        bangPrefab = GameConfig.BangObject;
    }

    // Update is called once per frame
    void Update()
    {
    }
    [ServerCallback]
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isEnable)
        {
            Throwable throwableIncoming = collision.gameObject.GetComponent<Throwable>();
            Breakable breakableIncoming = collision.gameObject.GetComponent<Breakable>();
            if (throwableIncoming != null && breakableIncoming != null)
            {
                if (throwableIncoming.IsBreakActive)
                {
                    breakableIncoming.Hit();
                    breakableIncoming.InstantiateBang();
                    Hit();
                }
            }

        }

    }
    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnable)
        {
            Throwable throwableIncoming = collision.gameObject.GetComponent<Throwable>();
            Breakable breakableIncoming = collision.gameObject.GetComponent<Breakable>();
            if (throwableIncoming != null && breakableIncoming != null)
            {
                if (throwableIncoming.IsBreakActive)
                {
                    breakableIncoming.Hit();
                    breakableIncoming.InstantiateBang();
                    Hit();
                }
            }

        }

    }
    [ServerCallback]
    public void Hit()
    {
        if(MaxToughness > 0)
        {
            toughness -= 1;
            if (IsDoor)
            {
                GameObject sfxHit = Instantiate(DoorHitSfx);
                NetworkServer.Spawn(sfxHit);
            }
            if (toughness <= 0)
            {
                if (IsDoor)
                {
                    GameObject sfxBreak = Instantiate(DoorBreakSfx);
                    NetworkServer.Spawn(sfxBreak);
                }
                Destroy(this.gameObject);
            }
        }
    }
    [ServerCallback]
    public void InstantiateBang()
    {
        if (bangPrefab != null)
        {
            GameObject bang = Instantiate<GameObject>(bangPrefab);
            bang.transform.position = transform.position;
            NetworkServer.Spawn(bang.gameObject);
        }
    }

}
