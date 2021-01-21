using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : MonoBehaviour
{
    private bool isRescued;
    private Grabbable grabbable;
    public bool IsRescued { get => isRescued; set => isRescued = value; }

    private AudioSource sfx;

    // Start is called before the first frame update

    void Start()
    {
        isRescued = false;
        grabbable = GetComponent<Grabbable>();
        sfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<RescueZone>() != null)
        {
            if (!isRescued)
            {
                sfx.Play();
                //add cat count?
            }
            //Destroy(this.gameObject);
            isRescued = true;
            grabbable.IsGrabbable = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<RescueZone>() != null)
        {
            //Destroy(this.gameObject);
            isRescued = false;
            grabbable.IsGrabbable = true;
        }
    }
}


