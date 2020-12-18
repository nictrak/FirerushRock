using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GrabHitbox : MonoBehaviour
{
    private List<Grabbable> grabbables;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Grabbable grabbable = collision.gameObject.GetComponent<Grabbable>();
        if (grabbable != null)
        {
            grabbables.Add(grabbable);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Grabbable grabbable = collision.gameObject.GetComponent<Grabbable>();
        if (grabbable != null)
        {
            grabbables.Remove(grabbable);
        }
    }
    private Grabbable CalNearest()
    {
        if (grabbables.Count == 0)
        {
            return null;
        }
        float shortestDistance = float.MaxValue;
        Grabbable temp;
        temp = grabbables[0];
        for (int i = 0; i < grabbables.Count; i++)
        {
            temp = grabbables[i];
            
        }
        return temp;
    }
}
