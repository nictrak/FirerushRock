﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFromFire : MonoBehaviour
{
    private float fire1Damage;
    private float fire2Damage;
    private float fire3Damage;
    private Life life;

    private List<Cell> cells;
    // Start is called before the first frame update
    void Start()
    {
        fire1Damage = 1;
        fire2Damage = 2;
        fire3Damage = 3;
        cells = new List<Cell>();
        life = GetComponent<Life>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {
        for(int i = 0; i < cells.Count; i++)
        {
            Damage(cells[i]);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Cell cell = collision.gameObject.GetComponent<Cell>();
        if (cell != null)
        {
            cells.Add(cell);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Cell cell = collision.gameObject.GetComponent<Cell>();
        if (cell != null)
        {
            cells.Remove(cell);
        }
    }
    private void Damage(Cell cell)
    {
        int x = (int)cell.GridPosition.x;
        int y = (int)cell.GridPosition.y;
        double fire1 = FireSystem.fire_1_array[y, x];
        double fire2 = FireSystem.fire_2_array[y, x];
        double fire3 = FireSystem.fire_3_array[y, x];
        if (fire1 != 0) 
        {
            life.Damage(fire1Damage);
        }
        if (fire2 != 0)
        {
            life.Damage(fire2Damage);
        }
        if (fire3 != 0)
        {
            life.Damage(fire3Damage);
        }
    }
}