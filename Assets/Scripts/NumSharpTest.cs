using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NumSharp;

public class NumSharpTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var x = np.arange(12);
        x = x.reshape(2, -1);
        Debug.Log(x);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
