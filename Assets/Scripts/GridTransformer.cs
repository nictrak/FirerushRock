using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTransformer : MonoBehaviour
{
    public static GridSystem ReferencedGrid;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindGridSystemLoop();
        Debug.Log(ReferencedGrid);

    }
    private void FindGridSystemLoop()
    {
        if(ReferencedGrid == null)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("GridSystem");
            if(temp != null)
            {
                ReferencedGrid = temp.GetComponent<GridSystem>();
            }
        }
    }
    public static Vector2 CalGridPosition(Vector2 realPosition)
    {
        Vector2 gridPosition = new Vector2();
        if (ReferencedGrid == null)
        {
            return gridPosition;
        }
        Vector2 originPosition2 = new Vector2(ReferencedGrid.OriginPosition.x, ReferencedGrid.OriginPosition.y);
        Vector2 basePosition = originPosition2 - ReferencedGrid.CellSize;
        float xDistance = realPosition.x - basePosition.x;
        float yDistance = basePosition.y - realPosition.y;
        gridPosition = new Vector2((int)(xDistance / ReferencedGrid.CellSize.x), (int)(yDistance / ReferencedGrid.CellSize.y));
        return gridPosition;
        
    }
}
