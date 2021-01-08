using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bang : MonoBehaviour
{
    public int Lifetime;
    private int counter;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(counter >= Lifetime)
        {
            Destroy(gameObject);
        }
        else
        {
            counter += 1;
        }
    }
}
