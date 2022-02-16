using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private GameObject brickPrefab;

    #endregion

    void Start()
    {
        GenerateGrid();
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