using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    private float lifePoint;

    public float MaxLifePoint;

    public float LifePoint { get => lifePoint; set => lifePoint = value; }

    // Start is called before the first frame update
    void Start()
    {
        lifePoint = MaxLifePoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
