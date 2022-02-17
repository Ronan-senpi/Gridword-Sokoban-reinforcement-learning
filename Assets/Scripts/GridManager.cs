using System;
using System.Collections;
using System.Collections.Generic;
using classes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    private Vector2Int gridSize = new Vector2Int(10, 10);
    public GridController gc { get; private set; }

    [SerializeField] private Transform playerContainer;
    
    [SerializeField] private GameObject groundPrefab; // 1
    [SerializeField] private GameObject pointPrefab; // 2
    [SerializeField] private GameObject cratePrefab; // 3
    [SerializeField] private GameObject holePrefab; //4
    [SerializeField] private GameObject arrowPrefab; //5
    [SerializeField] private GameObject characterPrefab; //6
    [SerializeField] private GameObject wallPrefab; //7
    private List<int> loadedGrid;

    List<int> gridOne = new List<int>()
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

    List<int> gridTwo = new List<int>()
    {
        7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
        7, 1, 1, 1, 7, 1, 1, 1, 1, 7,
        7, 1, 1, 1, 7, 1, 1, 1, 1, 7,
        7, 1, 1, 1, 7, 1, 1, 3, 1, 7,
        7, 1, 1, 1, 7, 1, 1, 1, 1, 7,
        7, 1, 1, 1, 7, 1, 1, 1, 1, 7,
        7, 1, 1, 1, 7, 1, 1, 1, 1, 7,
        7, 4, 1, 1, 1, 1, 1, 1, 1, 7,
        7, 1, 1, 1, 1, 1, 1, 1, 1, 7,
        7, 7, 7, 7, 7, 7, 7, 7, 7, 7
    };

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        gc = new GridController(gridTwo, Vector2Int.one);
        Instance = this;
    }

    void Start()
    {
        GenerateGrid(gc.CurrentGrid);
        GeneratePlayer(gc.PlayerPosition);
        SetCameraPosition();
    }

    private void GeneratePlayer(Vector2Int pp)
    {
        foreach (Transform child in playerContainer)
        {
            GameObject.Destroy(child.gameObject); 
        }
        InstantiateTiles(characterPrefab, pp, SpriteLayer.Character, playerContainer );
    }

    private void Update()
    {
        UpdateGrid();
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
       
        if (!gc.PlayerChange)
            return;
        
        GeneratePlayer(gc.PlayerPosition);
    }

    private void UpdateGrid()
    {
        if (!gc.GridChange)
            return;
        
        GenerateGrid(gc.CurrentGrid);
    }

    #region public



    #endregion public

    #region Private

    void SetCameraPosition()
    {
        Camera.main.transform.position = new Vector3(gridSize.x / 2.0f, (gridSize.y / 2.0f) - 0.5f, -10);
        Camera.main.orthographicSize = (gridSize.y / 2.0f) + 0.5f;
    }

    void GenerateGrid(List<int> matrix)
    {
        if (matrix.Count != 100)
        {
            Debug.LogError("Attention la matice n'est pas de la bonne taile, (" + matrix.Count + "/"
                           + gridSize.x * +gridSize.y + ")");
            return;
        }
        
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
        
        loadedGrid = matrix;
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                int cellValue = matrix[y * gridSize.x + x];
                //gridSize.y-y parce que si non c'est instentié a l'enver ! :o
                // -1 pour start a 0
                // GenerateCell((GridType) cellValue, new Vector2(x, gridSize.y-y-1));
                GenerateCell((GridType) cellValue, new Vector2(x, y));
            }
        }
    }

    void GenerateCell(GridType t, Vector2 pos)
    {
        if (t == GridType.Void)
            return;
        InstantiateTiles(groundPrefab, pos, SpriteLayer.Ground);
        switch (t)
        {
            case GridType.Point:
                InstantiateTiles(pointPrefab, pos, SpriteLayer.Prop);
                break;
            case GridType.Crate:
                InstantiateTiles(cratePrefab, pos, SpriteLayer.Prop);
                break;
            case GridType.Hole:
                InstantiateTiles(holePrefab, pos, SpriteLayer.Prop);
                break;
            case GridType.Arrow:
                InstantiateTiles(arrowPrefab, pos, SpriteLayer.Prop);
                break;
            case GridType.Character:
               // InstantiateTiles(characterPrefab, pos, SpriteLayer.Character);
                break;
            case GridType.Wall:
                InstantiateTiles(wallPrefab, pos, SpriteLayer.Prop);
                break;
            default:
                break;
        }
    }

    void InstantiateTiles(GameObject dd, Vector2 pos, SpriteLayer sl)
    {
        InstantiateTiles(dd,pos,sl,transform);
    }
    void InstantiateTiles(GameObject dd, Vector2 pos, SpriteLayer sl, Transform parent)
    {
        GameObject go = Instantiate(dd, pos, Quaternion.identity, parent);
        SpriteRenderer sr;
        if (!go.TryGetComponent(out sr))
        {
            Debug.LogAssertion("Il manque un SpriteRenderer");
            return;
        }

        sr.sortingOrder = (int) sl;
    }
    #endregion Private
}