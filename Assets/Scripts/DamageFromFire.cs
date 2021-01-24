using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DamageFromFire : NetworkBehaviour
{
    protected float fire1Damage;
    protected float fire2Damage;
    protected float fire3Damage;
    protected Life life;
    [SerializeField]
    private List<Cell> cells;
    // Start is called before the first frame update
    private void Awake()
    {
        cells = new List<Cell>();
    }
    void Start()
    {
        DamageSetup();
    }

    protected void DamageSetup()
    {
        fire1Damage = 0;
        fire2Damage = 2;
        fire3Damage = 3;
        life = GetComponent<Life>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {
        
    }
    private void LateUpdate()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if(cells[i] == null)
            {
                cells.RemoveAt(i);
            }
            else Damage(cells[i]);
        }
    }
    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Cell cell = collision.gameObject.GetComponent<Cell>();
        if (cell != null)
        {
            cells.Add(cell);
        }
    }
    [ServerCallback]
    private void OnTriggerExit2D(Collider2D collision)
    {
        Cell cell = collision.gameObject.GetComponent<Cell>();
        if (cell != null)
        {
            cells.Remove(cell);
        }
    }
    [ServerCallback]
    protected void Damage(Cell cell)
    {
        int x = (int)cell.GridPosition.x;
        int y = (int)cell.GridPosition.y;
        double heat = FireSystem.heat_array[y, x];

        if (heat >= 75)
        {
            life.Damage(fire3Damage);
        }
        else if (heat >= 50)
        {
            life.Damage(fire2Damage);
        }
        /*
        else if (fire1 != 0) 
        {
            life.Damage(fire1Damage);
        }
        */
    }
}
