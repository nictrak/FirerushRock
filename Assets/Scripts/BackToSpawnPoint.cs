using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToSpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Firefighter player = collision.gameObject.GetComponent<Firefighter>();
        if(player != null)
        {
            player.ToSpawnPoint();
        }
    }
}
