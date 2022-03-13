using System;
using System.Collections;
using System.Collections.Generic;
using classes;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    [Range(1,3)]
    [SerializeField] protected int LevelIndex =1;

    [Header("WARNING !! Edit size of any list can break the game!")] [SerializeField]
    private List<LevelInfos> levels = new List<LevelInfos>()
    {
        new LevelInfos(new Vector2Int(3, 6),
            new List<int>()
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 7, 7, 7, 7, 7, 0, 0, 0,
                0, 0, 7, 1, 1, 2, 7, 0, 0, 0,
                0, 0, 7, 1, 1, 2, 7, 0, 0, 0,
                0, 0, 7, 1, 1, 1, 7, 0, 0, 0,
                0, 0, 7, 7, 7, 7, 7, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            }),
        new LevelInfos(new Vector2Int(1, 1),
            new List<int>()
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
            },
            new List<Vector2Int>()
            {
                new Vector2Int(4, 5),
                new Vector2Int(4, 4),
            }),
        new LevelInfos(new Vector2Int(8,8),
            new List<int>()
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
            })
    };

    public LevelInfos SelectedLevel
    {
        get
        {
            LevelIndex = Mathf.Clamp(LevelIndex, 1, levels.Count);
            return levels[LevelIndex-1];
        }
    }

    public List<LevelInfos> Levels
    {
        get => levels;
    }
}