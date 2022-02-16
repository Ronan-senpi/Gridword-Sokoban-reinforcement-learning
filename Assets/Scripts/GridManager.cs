using System;
using System.Collections;
using System.Collections.Generic;
using classes;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    private Vector2Int gridSize = new Vector2Int(10, 10);

    [SerializeField] private GameObject groundPrefab; // 1
    [SerializeField] private GameObject pointPrefab; // 2
    [SerializeField] private GameObject cratePrefab; // 3
    [SerializeField] private GameObject holePrefab; //4
    [SerializeField] private GameObject arrowPrefab; //5
    [SerializeField] private GameObject characterPrefab; //6
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
        1, 1, 1, 0, 0, 0, 0, 1, 1, 1,
        1, 1, 1, 0, 0, 0, 0, 6, 1, 1,
        1, 1, 1, 0, 0, 0, 0, 1, 1, 1,
        1, 1, 1, 0, 0, 0, 0, 1, 1, 1,
        1, 1, 1, 0, 0, 0, 0, 1, 1, 1,
        1, 1, 1, 0, 0, 0, 0, 1, 1, 1,
        1, 1, 1, 0, 0, 0, 0, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1
    };

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        GenerateGrid(gridTwo);
        SetCameraPosition();
    }

    #region public

    /// <summary>
    /// Get information of tile
    /// </summary>
    /// <param name="pos">tile's coordinate</param>
    /// <returns>-1 if not find</returns>
    public int GetTile(Vector2Int pos)
    {
        int index = pos.y * gridSize.x + pos.x;
        if (index < 0
            || loadedGrid == null ||
            loadedGrid.Count < index ||
            !(0 <= pos.x && pos.x <= gridSize.x) ||
            !(0 <= pos.y && pos.y <= gridSize.y))
            return -1;
        return pos.y * gridSize.x + pos.x;
    }

    public Tuple<bool, GridType> canSteoOn(Vector3 pos)
    {
        //Get la value de la cell
        int cellValue = loadedGrid[GetTile(new Vector2Int((int) pos.x, (int) pos.y))];
        //Int to enum
        GridType gridType = (GridType) Enum.ToObject(typeof(GridType), cellValue);
        Tuple<bool, GridType> res;
        switch (gridType)
        {
            case GridType.Character:
            case GridType.Arrow:
            case GridType.Point:
            case GridType.Crate:
            case GridType.Hole:
            case GridType.Ground:
                res = new Tuple<bool, GridType>(true, gridType);
                break;
            case GridType.Wall:
            case GridType.Void:
                res = new Tuple<bool, GridType>(false, gridType);
                break;
            default:
                res = new Tuple<bool, GridType>(false, gridType);
                break;
        }

        return res;
    }

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
                InstantiateTiles(characterPrefab, pos, SpriteLayer.Character);
                break;
            default:
                break;
        }
    }

    void InstantiateTiles(GameObject dd, Vector2 pos, SpriteLayer sl)
    {
        GameObject go = Instantiate(dd, pos, Quaternion.identity, transform);
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