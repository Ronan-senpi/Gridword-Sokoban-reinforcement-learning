using System;
using System.Collections;
using System.Collections.Generic;
using classes;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    public GridController gc { get; private set; }

    [SerializeField] private Transform playerContainer;
    [SerializeField] private Transform crateContainer;
    [SerializeField] private Transform winScreen;

    [Header("Cells prefabs")] 
    
    [SerializeField] private GameObject groundPrefab; // 1
    [SerializeField] private GameObject pointPrefab; // 2
    [SerializeField] private GameObject cratePrefab; // 3
    [SerializeField] private GameObject holePrefab; //4
    [SerializeField] private GameObject arrowPrefab; //5
    [SerializeField] private GameObject characterPrefab; //6
    [SerializeField] private GameObject wallPrefab; //7
    [SerializeField] private GameObject doorPrefab; //8
[Header("debug settings")]
    [SerializeField] private Boolean showText = false;
    [SerializeField] private GameObject textPrefab;
    private List<int> loadedGrid;

    List<int> gridOne = new List<int>()
    {
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 7, 7, 7, 7, 7, 0, 0, 0,
        0, 0, 7, 1, 1, 8, 7, 0, 0, 0,
        0, 0, 7, 1, 1, 7, 7, 0, 0, 0,
        0, 0, 7, 1, 1, 1, 7, 0, 0, 0,
        0, 0, 7, 7, 7, 7, 7, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    List<int> gridTwo = new List<int>()
    {
        7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
        7, 1, 1, 1, 7, 1, 1, 8, 1, 7,
        7, 1, 1, 1, 7, 1, 1, 1, 1, 7,
        7, 1, 1, 1, 7, 1, 1, 1, 1, 7,
        7, 1, 1, 1, 7, 1, 1, 1, 1, 7,
        7, 1, 1, 1, 7, 1, 1, 1, 1, 7,
        7, 1, 1, 1, 7, 1, 1, 1, 1, 7,
        7, 4, 1, 1, 1, 1, 1, 1, 1, 7,
        7, 1, 1, 1, 1, 1, 1, 1, 1, 7,
        7, 7, 7, 7, 7, 7, 7, 7, 7, 7
    };

    private List<int> gridThree = new List<int>()
    {
        7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
        7, 1, 1, 1, 1, 7, 1, 1, 1, 7,
        7, 1, 7, 1, 1, 1, 1, 1, 4, 7,
        7, 1, 1, 1, 7, 7, 7, 1, 1, 7,
        7, 1, 1, 1, 7, 1, 7, 4, 1, 7,
        7, 1, 1, 1, 7, 1, 1, 1, 1, 7,
        7, 1, 1, 7, 7, 7, 7, 1, 1, 7,
        7, 7, 1, 1, 1, 1, 7, 1, 1, 7,
        7, 1, 1, 1, 7, 8, 7, 1, 1, 7,
        7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
    };
    private List<Vector2Int> cratesTwo = new List<Vector2Int>()
    {
        new Vector2Int(5, 5),
        new Vector2Int(6, 6),
    };

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        gc = new GridController(gridThree, new Vector2Int(8,8));
        Instance = this;
        
    }

    public void DisplayState()
    {
        
    }
    void Start()
    {
        GenerateGrid(gc.CurrentGrid);
        GeneratePlayer(gc.PlayerPosition);
        GenerateCrate(gc.CratesPositions);
        SetCameraPosition();
    }

    private void GenerateCrate(List<Vector2Int> cratesPositions)
    {
        if(cratesPositions == null)
            return;
        
        foreach (Transform child in crateContainer)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Vector2Int crate in cratesPositions)
        {
            InstantiateTiles(cratePrefab, crate, SpriteLayer.Character, crateContainer);
        }
    }

    private void GeneratePlayer(Vector2Int pp)
    {
        foreach (Transform child in playerContainer)
        {
            GameObject.Destroy(child.gameObject);
        }

        InstantiateTiles(characterPrefab, pp, SpriteLayer.Character, playerContainer);
    }

    private void Update()
    {
        UpdateGrid();
        UpdatePlayer();
        UpdateWin();
        UpdateCrate();
    }

    private void UpdateCrate()
    {
        if (!gc.CratesChange)
            return;

        GenerateCrate(gc.CratesPositions);
    }

    private void UpdateWin()
    {
        if (gc.Win)
        {
            winScreen.gameObject.SetActive(true);
        }
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
        Camera.main.transform.position = new Vector3(gc.GridSize.x / 2.0f, (gc.GridSize.y / 2.0f) - 0.5f, -10);
        Camera.main.orthographicSize = (gc.GridSize.y / 2.0f) + 0.5f;
    }

    void GenerateGrid(List<int> matrix)
    {
        // if (matrix.Count != 100)
        // {
        //     Debug.LogError("Attention la matice n'est pas de la bonne taile, (" + matrix.Count + "/"
        //                    + gridSize.x * +gridSize.y + ")");
        //     return;
        // }

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        loadedGrid = matrix;
        for (int x = 0; x < gc.GridSize.x; x++)
        {
            for (int y = 0; y < gc.GridSize.y; y++)
            {
                int cellValue = matrix[y * gc.GridSize.x + x];
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
            case GridType.Door:
                InstantiateTiles(doorPrefab, pos, SpriteLayer.Prop);
                break;
            default:
                break;
        }
    }

    void InstantiateTiles(GameObject dd, Vector2 pos, SpriteLayer sl)
    {
        InstantiateTiles(dd, pos, sl, transform);
    }

    void InstantiateTiles(GameObject dd, Vector2 pos, SpriteLayer sl, Transform parent)
    {
        GameObject go = Instantiate(dd, pos, Quaternion.identity, parent);
        go.transform.name = CreateCellName((int)pos.x, (int)pos.y);

        GameObject text = Instantiate(textPrefab, pos, Quaternion.identity, go.transform);
        text.transform.localPosition = new Vector3(-0.36f, 0.36f);
        TMP_Text tmp = text.GetComponent<TMP_Text>();
        // tmp.text = (pos.y * gc.GridSize.x + pos.x).ToString();

        SpriteRenderer sr;
        if (!go.TryGetComponent(out sr))
        {
            GameObject.Destroy(go);
            Debug.LogAssertion("Il manque un SpriteRenderer");
            return;
        }

        sr.sortingOrder = (int) sl;
    }

    private string CreateCellName(int x, int y)
    {
        return "(" + x + "," + y + ")"; 
    }
    #endregion Private

    
    public bool TryGetTextCell(int x, int y, out TMP_Text text)
    {
        text = null;
        Transform child = transform.Find(CreateCellName(x, y));
        if (child == null)
            return false;
        
        text = child.GetComponentInChildren<TMP_Text>();
        return text != null;
    }
    
}