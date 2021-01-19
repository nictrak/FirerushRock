using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GenerateCamera : NetworkBehaviour
{
    public Camera CameraPrefab;
    private Camera playerCamera;

    public Camera PlayerCamera { get => playerCamera; set => playerCamera = value; }

    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Instantiate(CameraPrefab, transform);
        //cam.gameObject.SetActive(false);
        cam.transform.localPosition = new Vector3(0, 0, -10);
        playerCamera = cam;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
