using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace classes
{
    public class GridController
    {
        public bool Win { get; private set; }

        #region Player

        public bool PlayerChange { get; private set; }
        private Vector2Int playerPosition;

        public Vector2Int PlayerPosition
        {
            get
            {
                PlayerChange = false;
                return playerPosition;
            }
        }

        public void MovePlayer(Vector2Int direction)
        {
            Vector2Int destination = playerPosition + direction;
            int indexDestination = destination.y * gridSize.x + destination.x;
            if (StepOnCrate(destination))
                if (!CanAndMoveCrate(destination, direction))
                    return;


            if (!CanStepOn(currentGrid[indexDestination]))
                return;
            playerPosition = destination;
            PlayerChange = true;
        }


        public bool CanStepOn(int indexToMove, bool playerMovement = true)
        {
            bool res;
            switch (indexToMove)
            {
                case (int) GridType.Arrow:
                case (int) GridType.Point:
                case (int) GridType.Hole:
                case (int) GridType.Ground:
                    res = true;
                    break;
                case (int) GridType.Door:
                    if (playerMovement)
                        Win = true;
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

        #endregion Player

        #region Grid

        public bool GridChange { get; private set; }

        public List<int> CurrentGrid
        {
            get
            {
                GridChange = false;
                return currentGrid;
            }
        }

        private List<int> currentGrid, tmpGrid, startGrid;
        private Vector2Int gridSize = new Vector2Int(10, 10);

        private void InitGrid(List<int> grid)
        {
            this.currentGrid = this.tmpGrid = this.startGrid = grid;
            playerPosition = new Vector2Int();
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
            return currentGrid[pos.y * gridSize.x + pos.x];
        }

        #endregion Grid

        #region Crate

        public bool CratesChange { get; private set; }
        private List<Vector2Int> cratesPositions;

        public List<Vector2Int> CratesPositions
        {
            get
            {
                CratesChange = false;
                return cratesPositions;
            }
        }

        private bool StepOnCrate(Vector2Int currentCell)
        {
            return cratesPositions.Any(x => x.x == currentCell.x && x.y == currentCell.y);
        }

        /// <summary>
        /// Check if can move crate & move it in case
        /// </summary>
        /// <param name="currentCell">Cell where we can found crate</param>
        /// <param name="direction">Direction of movement</param>
        /// <returns>True if can move crate and movement successed</returns>
        private bool CanAndMoveCrate(Vector2Int currentCell, Vector2Int direction)
        {
            Vector2Int crate = cratesPositions.FirstOrDefault(x => x.x == currentCell.x && x.y == currentCell.y);
            if (crate == null)
                return false;

            Vector2Int destination = crate + direction;
            int tileInfo = GetTile(destination);

            if (tileInfo == -1 || !CanStepOn(tileInfo, false))
                return false;

            if (!cratesPositions.Remove(crate))
                return false;

            crate += direction;
            cratesPositions.Add(crate);
            CratesChange = true;
            return true;
        }

        #endregion Crate

        public GridController(List<int> grid, Vector2Int playerPosition)
        {
            InitGrid(grid);
            this.playerPosition = playerPosition;
        }

        public GridController(List<int> grid)
        {
            InitGrid(grid);
        }

        public GridController(List<int> grid, Vector2Int playerPosition, List<Vector2Int> crates)
        {
            InitGrid(grid);
            this.playerPosition = playerPosition;
            this.cratesPositions = crates;
        }
    }
}