using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    private float lifePoint;

    public float MaxLifePoint;
    public GameObject LifeBar;
    public GameObject Entity;

    public float LifePoint { get => lifePoint; set => lifePoint = value; }

    // Start is called before the first frame update
    void Start()
    {
        lifePoint = MaxLifePoint;
        SyncLifeBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damagePoint)
    {
        lifePoint -= damagePoint;
        if(lifePoint <= 0)
        {
            Destroy(Entity);
        }
        else
        {
            SyncLifeBar();
        }
    }

    private void SyncLifeBar()
    {
        LifeBar.transform.localScale = new Vector3(lifePoint / MaxLifePoint, 0.1f, 1);
    }
}
