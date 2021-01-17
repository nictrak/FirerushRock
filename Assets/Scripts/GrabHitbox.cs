using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GrabHitbox : MonoBehaviour
{
    private List<Grabbable> grabbables;

    public List<Grabbable> Grabbables { get => grabbables; set => grabbables = value; }

    // Start is called before the first frame update
    void Start()
    {
        grabbables = new List<Grabbable>();
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
    public Grabbable CalNearest()
    {
        if (grabbables.Count == 0)
        {
            return null;
        }
        float shortestDistance = float.MaxValue;
        int index = -1;
        float distance;
        Grabbable temp;
        for (int i = 0; i < grabbables.Count; i++)
        {
            temp = grabbables[i];
            if(temp != null)
            {
                if (!temp.IsGrabed)
                {
                    distance = CalDistance(temp.transform.position);
                    if (shortestDistance > distance)
                    {
                        shortestDistance = distance;
                        index = i;
                    }
                }
            }
            else
            {
                grabbables.RemoveAt(i);
            }
        }
        if (index >= 0)
        {
            return grabbables[index];
        }
        return null;
    }
    private float CalDistance(Vector3 position)
    {
        Vector2 distanceVector =  transform.position - position;
        return distanceVector.magnitude;
    }
}
