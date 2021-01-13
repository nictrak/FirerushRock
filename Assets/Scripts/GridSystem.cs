using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridSystem : MonoBehaviour
{
    private List<List<Cell>> grids;
    public Vector3 OriginPosition;
    public Vector2 CellSize;
    public Vector2 GridSize;
    public Cell GridPrefab;
    public float BaseZ;

    public LevelControl LevelControl;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("asd");
        Debug.Log(FireSystem.wall_array);
        grids = new List<List<Cell>>();
        for (int j = 0; j < GridSize.y; j++)
        {
            List<Cell> newRow = new List<Cell>();
            for (int i = 0; i < GridSize.x; i++)
            {
                float xPosition = OriginPosition.x + i * CellSize.x;
                float yPosition = OriginPosition.y + -j * CellSize.y;
                Vector3 instatiatePosition = new Vector3(xPosition, yPosition, BaseZ);
                Cell newGrid = Instantiate<Cell>(GridPrefab);
                newGrid.transform.position = instatiatePosition;
                newGrid.GridPosition = new Vector2(i, j);
                newRow.Add(newGrid);
            }
            grids.Add(newRow);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
