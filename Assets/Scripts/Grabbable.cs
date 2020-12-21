using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    private bool isGrabed;
    private PlayerGrab grabber;
    public bool IsGrabed { get => isGrabed; set => isGrabed = value; }
    public PlayerGrab Grabber { get => grabber; set => grabber = value; }

    // Start is called before the first frame update
    void Start()
    {
        isGrabed = false;
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
    private void MoveWhenGrabed()
    {
        if (isGrabed)
        {
            transform.position = grabber.transform.position;
        }
    }
}
