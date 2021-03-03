using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HeatReducer : NetworkBehaviour
{
    public double ReduceRate;
    [SyncVar]
    private Vector2 moveVector;
    private int lifeTime;
    private Rigidbody2D rigidbody;
    public Vector2 MoveVector { get => moveVector; set => moveVector = value; }
    public int LifeTime { get => lifeTime; set => lifeTime = value; }

    private int timeCounter;
    // Start is called before the first frame update
    private void Awake()
    {
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
        CheckLifeTimeLoop();
        ReduceHeat();
        rigidbody.position += moveVector;
    }
    [ServerCallback]
    private void CheckLifeTimeLoop()
    {
        if (timeCounter == LifeTime)
        {
            Destroy(this.gameObject);
        }
        timeCounter += 1;
    }
    [ServerCallback]
    protected Vector2 GetGridPosition()
    {
        return GridTransformer.CalGridPosition(rigidbody.position);
    }
    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BreakWater>() != null)
        {
            Debug.Log("breaked");
            Debug.Log(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
    [ServerCallback]
    private void ReduceHeat()
    {
        Vector2 gridPosition = GetGridPosition();
        Reduce((int)gridPosition.x, (int)gridPosition.y, ReduceRate);
    }
    [ServerCallback]
    protected void Reduce(int x, int y, double rate)
    {
        if (FireSystem.isRun)
        {
            double heat = FireSystem.heat_array[y, x];
            FireSystem.heat_array[y, x] = heat - rate;
        }
    }
    /*[ServerCallback]
    private void ReduceHeat()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            HeatReduce(cells[i]);
        }
    }
    [ServerCallback]
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
    private void HeatReduce(Cell cell)
    {
        double heat = FireSystem.heat_array[(int)cell.GridPosition.y, (int)cell.GridPosition.x];
        FireSystem.heat_array[(int)cell.GridPosition.y, (int)cell.GridPosition.x] = heat - ReduceRate;
    }*/
    private void OnDestroy()
    {
        Debug.Log("Destroyed");
    }
}
