using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatReducer : MonoBehaviour
{
    public float ReduceRate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Cell cell = collision.gameObject.GetComponent<Cell>();
        if(cell != null)
        {
            HeatReduce(cell);
        }
    }
    private void HeatReduce(Cell cell)
    {
        FireSystem.heat_array[(int)cell.GridPosition.y, (int)cell.GridPosition.x] -= ReduceRate;
    }
}
