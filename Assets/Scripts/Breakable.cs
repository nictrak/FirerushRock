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
        toughness -= 1;
        if (toughness <= 0)
        {
            Destroy(this.gameObject);
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
