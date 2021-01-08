using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survivor : MonoBehaviour
{
    private bool isRescued;

    public bool IsRescued { get => isRescued; set => isRescued = value; }

    // Start is called before the first frame update

    void Start()
    {
        isRescued = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<RescueZone>() != null)
        {
            //Destroy(this.gameObject);
            isRescued = true;
        }
    }
}


