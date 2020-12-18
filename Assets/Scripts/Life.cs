using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NumSharp;

public class Life : MonoBehaviour
{
    public float lifePoint;

    // Start is called before the first frame update
    void Start()
    {
        var nd = np.full(5, 12);
        Debug.Log(nd);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
