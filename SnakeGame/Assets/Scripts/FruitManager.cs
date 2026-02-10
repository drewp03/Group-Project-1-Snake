using UnityEngine;
using System.Collections.Generic;

public class FruitManager : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject fruitPrefab;

    private Vector2Int fruitPosition;
    private Transform fruitTransform;
    
    
    //spawns fruit
    public void SpawnFruit(List<Vector2Int> occupiedPositions)
    {
        //removes old fruit if it exists
        if (fruitTransform != null)
        {
            Destroy(fruitTransform.gameObject);
            fruitTransform = null;
        }

        if(occupiedPositions == null)
            occupiedPositions = new List<Vector2Int>();

        int totalGridCells = gridManager.gridSize * gridManager.gridSize;
        if(occupiedPositions.Count >= totalGridCells)
        {
            Debug.Log("Snake has filled the grid");
            return;
        }

        do
        {
            fruitPosition = new Vector2Int(Random.Range(0,gridManager.gridSize),Random.Range(0,gridManager.gridSize));
        }while(occupiedPositions.Contains(fruitPosition));

        fruitTransform = Instantiate(fruitPrefab,gridManager.GridToWorld(fruitPosition),Quaternion.identity).transform;

        //Debug.Log($"Fruit Grid {fruitPosition} -> world {gridManager.GridToWorld(fruitPosition)}");
    }

    public Vector2Int GetFruitPosition()
    {
        return fruitPosition;
    }
}
