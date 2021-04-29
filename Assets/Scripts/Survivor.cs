using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Survivor : NetworkBehaviour
{
    [SerializeField]
    [SyncVar]
    private bool isRescued;
    private Grabbable grabbable;
    public bool IsRescued { get => isRescued; set => isRescued = value; }

    private AudioSource sfx;
    private Score Score;

    // Start is called before the first frame update

    void Start()
    {
        isRescued = false;
        grabbable = GetComponent<Grabbable>();
        sfx = GetComponent<AudioSource>();
        SetupScore();
    }
    [ServerCallback]
    private void SetupScore()
    {
        Score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
    }
    // Update is called once per frame
    void Update()
    {

    }
    [ClientRpc]
    public void RpcPlaySfx()
    {
        if(isRescued) sfx.Play();
    }
    [ServerCallback]
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<RescueZone>() != null)
        {
            if (!isRescued)
            {
                RpcPlaySfx();
                Score.AddCatRecued();
                //add cat count?
            }
            //Destroy(this.gameObject);
            isRescued = true;
            grabbable.IsGrabbable = false;
        }
    }
    [ServerCallback]
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<RescueZone>() != null)
        {
            //Destroy(this.gameObject);
            isRescued = false;
            grabbable.IsGrabbable = true;
        }
    }
    [ServerCallback]
    private void OnDestroy()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Score");
        if(obj != null)
        {
            Score = obj.GetComponent<Score>();
            Score.AddCatDied();
        }
    }
}


