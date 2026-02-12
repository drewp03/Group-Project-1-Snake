using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SnakeMovement : MonoBehaviour
{
    public FruitManager fruitManager;
    public GridManager gridManager;
    public GameObject headPrefab;
    public GameObject segmentPrefab;
    public float moveInterval = 0.5f;
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public static float startingMoveInterval;
    public AudioSource munch;



    //2 seperate lists to track the where the snake is (snakePositions) and to track the segments of the snake
    private List<Vector2Int> snakePositions = new List<Vector2Int>();
    private List<Transform> snakeSegments = new List<Transform>();

    private Vector2Int direction = Vector2Int.right;

    //how long it takes for the snake to move
    private float moveTimer;

    private void Start()
    {
        moveInterval = startingMoveInterval;
        //adding the head to the positions
        Vector2Int startPosition = new Vector2Int(0,0);
        snakePositions.Add(startPosition);

        //adding head to the segments
        Transform head = Instantiate(headPrefab, gridManager.GridToWorld(startPosition), Quaternion.identity).transform;
        snakeSegments.Add(head);

        fruitManager.SpawnFruit(snakePositions);

        UpdateScoreUI();
    }

    private void Update()
    {
        HandleInput();

        //adding time to how long you since have moved
        moveTimer += Time.deltaTime;

        //if the time since you have last moved is longer than the move interval, you move
        if (moveTimer >= moveInterval)
        {
            moveTimer = 0f;
            MoveSnake();
        }


    }

    private void UpdateScoreUI()
    {
        scoreText.text = $"{score}";
    }

    //movement handling
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2Int.down)
            direction = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2Int.right)
            direction = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2Int.up)
            direction = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2Int.left)
            direction = Vector2Int.right;

    }

    private void MoveSnake()
    {
        
        Vector2Int newHeadPosition = snakePositions [0] + direction;

        //checking to see if the snake is still inside of the grid, if it is not, the game ends
        if (!gridManager.IsInsideGrid(newHeadPosition))
        {
            GameOver();
            return;
        }

        
        bool ateFruit = newHeadPosition == fruitManager.GetFruitPosition();
        //if ate fruit is true, the tail does not move so you check all segments
        //if ate fruit is not true, the tail moves so you check all segments except the tail
        int segmentsToCheck = ateFruit ? snakePositions.Count : snakePositions.Count - 1;
        for(int i=0; i<segmentsToCheck; i++)
        {
            if(snakePositions[i] == newHeadPosition)
            {
                GameOver();
                return;
            }
        }

        //add new head
        snakePositions.Insert(0,newHeadPosition);

        //only remove the tail if you have not ate the fruit
        if(!ateFruit)
        {
            snakePositions.RemoveAt(snakePositions.Count - 1);
        }

        else
        {
            GrowSnake();
            fruitManager.SpawnFruit(snakePositions);

            munch.Play();

            if (moveInterval == 0.5f)
            {
                score += 1;
                ScoreManager.AddScore(1);
            }

            else
            {
                score += 2;
                ScoreManager.AddScore(2);
            }

            UpdateScoreUI();
            Debug.Log(score);
        }
        
        //function to make the snake move in game view
        UpdateVisuals();

        //if the snake has filled out the map, continue on
        if(snakePositions.Count >= gridManager.gridSize * gridManager.gridSize)
        {
            GridSizeUp();
        }
    }

    private void GridSizeUp()
    {
        if (gridManager.gridSize<10)
            gridManager.gridSize++;

        //THIS IS THE WIN SCRIPT IF THIS RUNS THEY HAVE BEATEN THE GAME
        else
        {
            SceneManager.LoadSceneAsync(4);
            Debug.Log("Congratulations, YOU WIN!!!!");
            enabled = false;

        }


        foreach(Transform segment in snakeSegments)
            Destroy(segment.gameObject);

        snakeSegments.Clear();
        snakePositions.Clear();

        direction = Vector2Int.right;
        moveTimer = 0f;

        //reset the head in the new grid
        Vector2Int startPosition = new Vector2Int(0,0);
        snakePositions.Add(startPosition);
        Transform head = Instantiate(headPrefab, gridManager.GridToWorld(startPosition), Quaternion.identity).transform;
        snakeSegments.Add(head);

        //generate the new grid and new offset
        gridManager.UpdateGridOffset();
        gridManager.GenerateGrid();

        //spawns fruit
        fruitManager.SpawnFruit(snakePositions);
    }

    private void GrowSnake()
    {
        Transform newSegment = Instantiate(segmentPrefab, gridManager.GridToWorld(snakePositions[snakePositions.Count - 1]),
        Quaternion.identity).transform;

        snakeSegments.Add(newSegment);
    }

    private void UpdateVisuals()
    {
        for(int i=0;i<snakeSegments.Count;i++)
        {
            snakeSegments[i].position = gridManager.GridToWorld(snakePositions[i]);
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over");

        SceneManager.LoadSceneAsync(3);

        enabled = false;
    }
}
