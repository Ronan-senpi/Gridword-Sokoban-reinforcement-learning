using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace classes
{
    public class GridController
    {
        public List<int> CurrentGrid
        {
            get
            {
                GridChange = false;
                return currentGrid;
            }
        }

        public bool PlayerChange { get; private set; }
        private Vector2Int playerPosition;
        public Vector2Int PlayerPosition {
            get
            {
                PlayerChange = false;
                return playerPosition;
            }
        }
        public bool GridChange { get; private set; }

        private List<int> currentGrid, tmpGrid, startGrid;
        private Vector2Int gridSize = new Vector2Int(10, 10);

        public GridController(List<int> grid)
        {
            InitGrid(grid);
        }

        private void InitGrid(List<int> grid)
        { 
            this.currentGrid = this.tmpGrid = this.startGrid = grid;
            playerPosition = new Vector2Int();
        }

        public GridController(List<int> grid, Vector2Int playerPosition)
        {
            InitGrid(grid);
            this.playerPosition = playerPosition;
        }

        
        public void Move(Vector2Int cellToMove, Vector2Int destination)
        {
            List<int> tmp = currentGrid;

            int indexDestination = destination.y * gridSize.x + destination.x;

            if (!canSteoOn(tmp[indexDestination]))
                return;

            GridChange = true;
            int indexToMove = cellToMove.y * gridSize.x + cellToMove.x;
            int val = tmp[indexToMove];
            tmp[indexToMove] = (int) GridType.Ground;
            tmp[indexDestination] = val;
            currentGrid = tmp;
        }
        
        public void MovePlayer(Vector2Int direction)
        {
            Vector2Int destination = playerPosition + direction;
            int indexDestination = destination.y * gridSize.x + destination.x;

            if (!canSteoOn(currentGrid[indexDestination]))
                return;
            playerPosition = destination;
            PlayerChange = true;
        }

        public bool canSteoOn(int indexToMove)
        {
            bool res;
            switch (indexToMove)
            {
                // case (int) GridType.Character:
                case (int) GridType.Crate:
                case (int) GridType.Arrow:
                case (int) GridType.Point:
                case (int) GridType.Hole:
                case (int) GridType.Ground:
                    res = true;
                    break;
                case (int) GridType.Wall:
                case (int) GridType.Void:
                    res = false;
                    break;
                default:
                    res = false;
                    break;
            }

            return res;
        }

        /// <summary>
        /// Get information of tile
        /// </summary>
        /// <param name="pos">tile's coordinate</param>
        /// <returns>-1 if not find</returns>
        public int GetTile(Vector2Int pos)
        {
            if (0 > pos.y || pos.y >= gridSize.y)
            {
                return -1;
            }

            if (0 > pos.x || pos.x >= gridSize.x)
            {
                return -1;
            }

            int index = pos.y * gridSize.x + pos.x;
            if (0 > index || index >= currentGrid.Count)
                return -1;
            return pos.y * gridSize.x + pos.x;
        }
    }
}