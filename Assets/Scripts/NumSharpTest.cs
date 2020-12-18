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
        var y = x[":, 1:5"];
        Debug.Log(y);
        var z = np.zeros(16);
        z = z.reshape(4, -1);
        Debug.Log(z);
        z["1:3,:"] = y;
        Debug.Log(z);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
