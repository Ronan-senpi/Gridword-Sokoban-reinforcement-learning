using System;
using System.Collections.Generic;
using UnityEngine;

namespace classes
{
    [System.Serializable]
    public class LevelInfos
    {
        public LevelInfos(Vector2Int playerPosition, List<int> grid)
        {
            
            this.playerPosition = playerPosition;
            this.grid = grid;
        }
        
        public LevelInfos(Vector2Int playerPosition, List<int> grid, Vector2Int size, List<Vector2Int> cratesPosition = null)
        {
            
            this.playerPosition = playerPosition;
            this.grid = grid;
            this.gridSize = size;
            this.cratesPosition = cratesPosition;
        }
        
        [Header("WARNING !! Edit size of any list can break the game!")]
        [ReadOnly] [SerializeField] protected Vector2Int playerPosition;
        
        [ReadOnly] [SerializeField] protected List<int> grid;
        [ReadOnly] [SerializeField] protected Vector2Int gridSize = new Vector2Int(10,10);

        [ReadOnly] [SerializeField] protected List<Vector2Int> cratesPosition;

        public Vector2Int GridSize
        {
            get => gridSize;
        }
        
        public Vector2Int PlayerPosition
        {
            get => playerPosition;
        }

        public List<int> Grid
        {
            get => grid;
        }

        public List<Vector2Int> CratesPosition
        {
            get => cratesPosition;
        }
    }
}