using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    private int toughness;

    public int MaxToughness;

    public SpriteRenderer BangPrefab;

    // Start is called before the first frame update
    void Start()
    {
        toughness = MaxToughness;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Throwable throwable = collision.gameObject.GetComponent<Throwable>();
        BreakThrow breakThrow = collision.gameObject.GetComponent<BreakThrow>();
        if (throwable != null)
        {
            if (throwable.IsBreakActive)
            {
                Hit();
            }
        }
        throwable = GetComponent<Throwable>();
        if (throwable != null && breakThrow != null)
        {
            if (throwable.IsBreakActive)
            {
                InstantiateBang();
                Hit();
            }
        }
    }

    private void Hit()
    {
        toughness -= 1;
        if (toughness <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void InstantiateBang()
    {
        if (BangPrefab != null)
        {
            SpriteRenderer bang = Instantiate<SpriteRenderer>(BangPrefab);
            bang.transform.position = transform.position;
        }
    }

}
