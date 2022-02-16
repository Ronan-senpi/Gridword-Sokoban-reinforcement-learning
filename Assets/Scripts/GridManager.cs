using System.Collections;
using System.Collections.Generic;
using classes;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    #region Members

    [Header("Grid Setup")]
    //number of tile in grid (width & height)
    [SerializeField] private Vector2 gridDimensions;

    //Tiles for ground
    [SerializeField] private List<GameObject> ground;

    [Space(10)]
    
    [Header("Victory Setup")] [SerializeField]
    private List<Vector2> winPointsCoordonates;

    [SerializeField] private GameObject winPointsPrefab;
    
    [Space(10)]
    
    [Header("Brick Setup")] [SerializeField]
    private List<Vector2> brickCoordinates;


    #endregion

    private int gridHeight = 10;
    private int gridWidth = 10;

    [SerializeField] private GameObject groundPrefab; // 1
    [SerializeField] private GameObject pointPrefab; // 2
    [SerializeField] private GameObject cratePrefab; // 3
    [SerializeField] private GameObject holePrefab; //4
    [SerializeField] private GameObject arrowPrefab; //5
    [SerializeField] private GameObject characterPrefab;


   
    
    
    void Start()
    {
        var gridOne = new List<int>()
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
            0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
            0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
            0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
            0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
            0, 0, 0, 1, 1, 1, 1, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        };
        GenerateGrid();
        SetCameraPosition();
    }

    void SetCameraPosition()
    {   
        Camera.main.transform.position= new Vector3(gridDimensions.x / 2, (gridDimensions.y/2)-0.5f, -10);
        Camera.main.orthographicSize = (gridDimensions.y / 2) +0.5f;

    }

    void GenerateGrid(List<int> matrix)
    {
        if (matrix.Count > 100)
        {
            Debug.LogError("Attention la matice est trop grande, ("+matrix.Count +"/"+gridHeight*+gridWidth+")");
            return;
        }
        for (int i = 0; i < 20; i++)
            for (int j = 0; j < 20; j++)
            {
                int cellValue = matrix[i * gridWidth * j];
                
                GenerateCell((GridType)cellValue, new Vector2());
                
            }
    }

    void GenerateCell(GridType t, Vector3 pos)
    {
        if(t == GridType.Void)
            return;

        GameObject currentGround = Instantiate(groundPrefab, pos, Quaternion.identity, transform);
        
        switch (t)
        {
            case GridType.Point:
                break;
            case GridType.Crate:
                break;
            case GridType.Hole:
                break;
            case GridType.Arrow:
                break;
            
            case GridType.Ground:
            default:
                break;
        }
    }

    void SetSpriteRendererLayer()   
    {
        
    }
    void GenerateGrid()
    {
        for (int i = 0; i < gridDimensions[0]; i++)
        {
            for (int j = 0; j < gridDimensions[1]; j++)
            {
                Vector3 tilePosition = transform.position;
                tilePosition.x += i; //- Mathf.Floor(gridDimensions[0] / 2 + 0.5f);
                tilePosition.y += j; // - Mathf.Floor(gridDimensions[1] / 2 + 0.5f);
                GameObject currentGround = Instantiate(ground[0], tilePosition, Quaternion.identity, transform);
                currentGround.name = "Tile " + i + j;
            }
        }

        for (int i = 0; i < winPointsCoordonates.Count; i++)
        {
            Vector2 victoryPosition = Vector2.zero;
            victoryPosition.x = winPointsCoordonates[i][0];
            victoryPosition.y = winPointsCoordonates[i][1];
            Instantiate(winPointsPrefab, victoryPosition, Quaternion.identity, transform);
        }
        
        for (int i = 0; i < brickCoordinates.Count; i++)
        {
            Vector2 brickPosition = Vector2.zero;
            brickPosition.x = brickCoordinates[i][0];
            brickPosition.y = brickCoordinates[i][1];
            Instantiate(brickPrefab, brickPosition, Quaternion.identity, transform);
        }
    }
}