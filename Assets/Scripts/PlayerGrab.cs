using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{

    public GameObject leftGrab;
    public GameObject rightGrab;
    public GameObject upGrab;
    public GameObject downGrab;
    public Grabbable grabedObject;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseObject()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            grabedObject.Use();
        }
    }
}
