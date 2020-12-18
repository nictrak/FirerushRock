using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    private string direction;
    private GameObject usedGrab;

    public GameObject LeftGrab;
    public GameObject RightGrab;
    public GameObject UpGrab;
    public GameObject DownGrab;
    public Grabbable GrabedObject;

    public string Direction { get => direction; set => direction = value; }
    public GameObject UsedGrab { get => usedGrab; set => usedGrab = value; }

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
            GrabedObject.Use();
        }
    }
    public void SelectGrab()
    {
        if(Direction == "left")
        {
            usedGrab = LeftGrab;
        }
        else if (Direction == "rigth")
        {
            usedGrab = RightGrab;
        }
        else if (Direction == "up")
        {
            usedGrab = UpGrab;
        }
        else if (Direction == "down")
        {
            usedGrab = DownGrab;
        }
    }
}
