using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private int toughness;

    public int MaxToughness;
    // Start is called before the first frame update
    void Start()
    {
        toughness = MaxToughness;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        Throwable throwable = collision.gameObject.GetComponent<Throwable>();
        if (throwable != null)
        {
            Debug.Log("Not null");
            if (throwable.IsBreakActive)
            {
                Debug.Log("Throwed");
                toughness -= 1;
                if(toughness <= 0)
                {
                    GetComponent<SpriteRenderer>().enabled = false;
                    GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }
}
