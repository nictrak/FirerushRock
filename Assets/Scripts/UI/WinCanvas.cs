using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class WinCanvas : NetworkBehaviour
{
    public GameObject Canvas;
    // Start is called before the first frame update
    void Start()
    {
        Canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    [ClientRpc]
    public void Win()
    {
        Canvas.SetActive(true);
    }
}
