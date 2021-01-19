using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class Life : NetworkBehaviour
{
    [SyncVar]
    private float lifePoint;
    private float maxLocalScale;
    private bool isRegen;
    private int regenCounter;


    public float MaxLifePoint;
    public GameObject LifeBar;
    public float RegenPoint;
    public int NonRegenTime;
    public bool IsPlayer;
    public float LifePoint { get => lifePoint; set => lifePoint = value; }

    // Start is called before the first frame update
    void Start()
    {
        if (LifeBar != null) maxLocalScale = LifeBar.transform.localScale.x;
        lifePoint = MaxLifePoint;
        SyncLifeBar();
        isRegen = true;
        regenCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {
        OpenRegen();
        RegenLifePoint();
        SyncLifeBar();
    }
    [ServerCallback]
    public void Damage(float damagePoint)
    {
        lifePoint -= damagePoint;
        if(lifePoint <= 0)
        {
            if (!IsPlayer) Destroy(gameObject);
            else GetComponent<Firefighter>().ToSpawnPoint();
        }
        isRegen = false;
        regenCounter = 0;
    }
    [ServerCallback]
    private void SyncLifeBar()
    {
        if(LifeBar != null)
        LifeBar.transform.localScale = new Vector3(lifePoint / MaxLifePoint * maxLocalScale, LifeBar.transform.localScale.y, 1);
    }
    [ServerCallback]
    private void RegenLifePoint()
    {
        if(lifePoint < MaxLifePoint && isRegen)
        {
            lifePoint += RegenPoint;
            if (lifePoint > MaxLifePoint)
            {
                lifePoint = MaxLifePoint;
            }
        }
    }
    [ServerCallback]
    private void OpenRegen()
    {
        if (!isRegen)
        {
            if (regenCounter >= NonRegenTime)
            {
                isRegen = true;
                regenCounter = 0;
            }
            else
            {
                regenCounter += 1;
            }
        }
    }
}
