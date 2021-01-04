using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefighter : MonoBehaviour
{
    private Rigidbody2D rigidbody;

    public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
