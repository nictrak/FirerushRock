using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    // Start is called before the first frame update
    public int counter;
    private int count;
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (count == counter)
        {
            Destroy(this.gameObject);
        }
        count++;
    }
}
