using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Firefighter TrackedFirefighter;
    public float BaseZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if(TrackedFirefighter != null)
        {
            Vector3 trackedPosition = TrackedFirefighter.Rigidbody.position;
            transform.position = new Vector3(trackedPosition.x, trackedPosition.y, BaseZ);
        }
    }
}
