using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Life : MonoBehaviour
{
    private float lifePoint;
    private float maxLocalScale;
    private bool isRegen;
    private int regenCounter;

    public float MaxLifePoint;
    public GameObject LifeBar;
    public GameObject Entity;
    public float RegenPoint;
    public int NonRegenTime;
    public float LifePoint { get => lifePoint; set => lifePoint = value; }

    // Start is called before the first frame update
    void Start()
    {
        maxLocalScale = LifeBar.transform.localScale.x;
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
    public void Damage(float damagePoint)
    {
        lifePoint -= damagePoint;
        if(lifePoint <= 0)
        {
            Destroy(Entity);
            SceneManager.LoadScene("Menu");
        }
        isRegen = false;
        regenCounter = 0;
    }

    private void SyncLifeBar()
    {
        LifeBar.transform.localScale = new Vector3(lifePoint / MaxLifePoint * maxLocalScale, LifeBar.transform.localScale.y, 1);
    }

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
