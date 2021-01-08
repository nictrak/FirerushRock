using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatReducer : MonoBehaviour
{
    public double ReduceRate;
    private List<Cell> cells;
    private Vector2 moveVector;
    private int lifeTime;
    private Rigidbody2D rigidbody;
    public Vector2 MoveVector { get => moveVector; set => moveVector = value; }
    public int LifeTime { get => lifeTime; set => lifeTime = value; }

    private int timeCounter;
    // Start is called before the first frame update
    private void Awake()
    {
        cells = new List<Cell>();
    }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        timeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void FixedUpdate()
    {
        if(timeCounter == LifeTime)
        {
            Destroy(this.gameObject);
        }
        for (int i = 0; i < cells.Count; i++)
        {
            HeatReduce(cells[i]);
        }
        rigidbody.position += moveVector;
        timeCounter += 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BreakWater>() != null)
        {
            Debug.Log("breaked");
            Debug.Log(collision.gameObject);
            Destroy(this.gameObject);
        }
        Cell cell = collision.gameObject.GetComponent<Cell>();
        if(cell != null)
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
    private void HeatReduce(Cell cell)
    {
        double heat = FireSystem.heat_array[(int)cell.GridPosition.y, (int)cell.GridPosition.x];
        FireSystem.heat_array[(int)cell.GridPosition.y, (int)cell.GridPosition.x] = heat - ReduceRate;
    }
}
