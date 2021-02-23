using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GrabHitbox : MonoBehaviour
{
    [SerializeField]
    private List<Grabbable> grabbables;
    [SerializeField]
    private List<GameObject> breakWaters;
    public List<Grabbable> Grabbables { get => grabbables; set => grabbables = value; }
    public List<GameObject> BreakWaters { get => breakWaters; set => breakWaters = value; }

    // Start is called before the first frame update
    void Start()
    {
        grabbables = new List<Grabbable>();
        breakWaters = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Grabbable grabbable = collision.gameObject.GetComponent<Grabbable>();
        BreakWater breaker = collision.gameObject.GetComponent<BreakWater>();
        if (grabbable != null)
        {
            grabbables.Add(grabbable);
        }
        if(breaker != null || grabbable != null)
        {
            breakWaters.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Grabbable grabbable = collision.gameObject.GetComponent<Grabbable>();
        BreakWater breaker = collision.gameObject.GetComponent<BreakWater>();
        if (grabbable != null)
        {
            grabbables.Remove(grabbable);
        }
        if (breaker != null || grabbable != null)
        {
            breakWaters.Remove(collision.gameObject);
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
    public bool IsBreakWatersEmpty()
    {
        if(breakWaters.Count <= 0)
        {
            return true;
        }
        return false;
    }
    private float CalDistance(Vector3 position)
    {
        Vector2 distanceVector =  transform.position - position;
        return distanceVector.magnitude;
    }
}
