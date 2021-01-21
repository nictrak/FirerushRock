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
        fire1Damage = 1;
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
            Damage(cells[i]);
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
        double fire1 = FireSystem.fire_1_array[y, x];
        double fire2 = FireSystem.fire_2_array[y, x];
        double fire3 = FireSystem.fire_3_array[y, x];

        if (fire3 != 0)
        {
            life.Damage(fire3Damage);
        }
        else if (fire2 != 0)
        {
            life.Damage(fire2Damage);
        }
        else if (fire1 != 0) 
        {
            life.Damage(fire1Damage);
        }
    }
}
