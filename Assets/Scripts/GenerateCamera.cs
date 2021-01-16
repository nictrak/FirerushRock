using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GenerateCamera : NetworkBehaviour
{
    public Camera CameraPrefab;
    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            Camera cam = Instantiate(CameraPrefab, transform);
            cam.transform.localPosition = new Vector3(0, 0, -10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
