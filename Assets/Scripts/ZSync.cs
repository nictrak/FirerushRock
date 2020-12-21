using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZSync : MonoBehaviour
{
    private float lavitateHeight;

    public float SpriteHeight;

    public float LavitateHeight { get => lavitateHeight; set => lavitateHeight = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Sync();
    }
    private void Sync()
    {
        Vector3 presentPosition = transform.position;
        transform.position = new Vector3(presentPosition.x, presentPosition.y, CalZ());
    }
    private float CalZ()
    {
        Vector3 presentPosition = transform.position;
        return (presentPosition.y - SpriteHeight / 2 - lavitateHeight) / 10;
    }
    public Vector2 GetPlanarPosition()
    {
        return new Vector2(transform.position.x, transform.position.y - SpriteHeight / 2);
    }
}
