using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform gridParent;
    private Vector2 gridOffset;

    public int gridSize = 6;

    private void Awake()
    {
        gridOffset = new Vector2(-gridSize / 2f + 0.5f, -gridSize / 2f + 0.5f);
    }

    private void Start()
    {
        GenerateGrid();

        //Debug.Log($"Grid Offset: {gridOffset}");
    }

    public void GenerateGrid()
    {
        //stop if there are no tiles
        if (tilePrefab == null || gridParent == null)
            return;

        //clears old grid
        foreach(Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

        //x loop and y loop; at 0,0 it generates a square, then 0,1 etc. until it hits the grid size
        //this is how the game can scale up
        for(int x=0; x<gridSize; x++)
        {
            for(int y=0; y<gridSize; y++)
            {
                Vector3 position = new Vector3(x+gridOffset.x, y+gridOffset.y, 0f);
                Instantiate(tilePrefab, position, Quaternion.identity, gridParent);
            }
        }
    }

    //converts regular grid coords (0,0 or 3,5) to the unity world
    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        //Debug.Log($"GridToWorld called: {gridPosition} offset {gridOffset}");

        return new Vector3(gridPosition.x + gridOffset.x, gridPosition.y + gridOffset.y, 0f);
    }

    //boolean function to check whether or not the snake is in the grid
    public bool IsInsideGrid(Vector2Int gridPosition)
    {
        return gridPosition.x >= 0 &&
            gridPosition.y >= 0 &&
            gridPosition.x < gridSize &&
            gridPosition.y < gridSize;
    }

    public void UpdateGridOffset()
    {
        gridOffset = new Vector2(-gridSize / 2f + 0.5f, -gridSize / 2f + 0.5f);
    }
}
