using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DealDamage : NetworkBehaviour
{
    protected float fire1Damage;
    protected float fire2Damage;
    protected float fire3Damage;
    protected Life life;
    private Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        DamageSetup();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        Vector2 gridPosition = GetGridPosition();
        Damage((int)gridPosition.x, (int)gridPosition.y);
    }
    protected void DamageSetup()
    {
        fire1Damage = 0;
        fire2Damage = 10;
        fire3Damage = 20;
        life = GetComponent<Life>();
    }
    [ServerCallback]
    protected Vector2 GetGridPosition()
    {
        return GridTransformer.CalGridPosition(rigidbody.position);
    }

    [ServerCallback]
    protected void Damage(int x, int y)
    {
        if (FireSystem.isRun)
        {
            double heat = FireSystem.heat_array[y, x];

            if (heat >= 75)
            {
                life.Damage(fire3Damage);
            }
            else if (heat >= 50)
            {
                life.Damage(fire2Damage);
            }
        }
    }
}
