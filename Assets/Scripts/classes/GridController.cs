using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace classes
{
    public class GridController
    {
        public bool Win { get; set; }

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
            int indexDestination = destination.y * GridSize.x + destination.x;
            if (cratesPositions != null && StepOnCrate(cratesPositions, destination, out _))
                if (!CanMoveCrate(destination, direction))
                    return;
                else
                {
                    MoveCrate(destination, direction);
                }


            if (!CanStepOn(currentGrid[indexDestination]))
                return;
            playerPosition = destination;
            PlayerChange = true;
        }


        public bool CanStepOn(Vector2Int pos, Direction dir)
        {
            Vector2Int tmpDes = GetNextPosition(pos, dir, out _);
            return CanStepOn(GetTile(tmpDes), false);
        }

        public bool CanStepOn(int cellValue, bool playerMovement = true)
        {
            bool res;
            switch (cellValue)
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

        public bool GridChange { get; private set; } = false;

        public List<int> StartGrid
        {
            get { return startGrid; }
        }

        public List<int> CurrentGrid
        {
            get
            {
                GridChange = false;
                return currentGrid;
            }
        }

        private List<int> currentGrid, tmpGrid, startGrid;
        public Vector2Int GridSize { get; private set; } = new Vector2Int(10, 10);

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
            if (0 > pos.y || pos.y >= GridSize.y)
            {
                return -1;
            }

            if (0 > pos.x || pos.x >= GridSize.x)
            {
                return -1;
            }

            int index = pos.y * GridSize.x + pos.x;
            if (0 > index || index >= currentGrid.Count)
                return -1;
            return currentGrid[index];
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

        public List<Vector2Int> pointsPosition { get; set; }

        public static bool StepOnCrate(List<Vector2Int> crates, Vector2Int currentCell, out int index)
        {
            index = -1;
            if (crates.Contains(currentCell))
            {
                index = crates.IndexOf(currentCell);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if can move crate & move it in case
        /// </summary>
        /// <param name="currentCell">Cell where we can found crate</param>
        /// <param name="direction">Direction of movement</param>
        /// <returns>True if can move crate and movement successed</returns>
        public bool CanMoveCrate(Vector2Int currentCell, Vector2Int direction)
        {
            Vector2Int crate = cratesPositions.FirstOrDefault(x => x.x == currentCell.x && x.y == currentCell.y);
            if (crate == null)
                return false;

            Vector2Int destination = crate + direction;
            int tileInfo = GetTile(destination);

            if (tileInfo == -1 || !CanStepOn(tileInfo, false) || cratesPositions.Any(x => x == destination))
                return false;


            return true;
        }

        public void MoveCrate(Vector2Int crate, Vector2Int direction)
        {
            if (!cratesPositions.Remove(crate))
                return;

            crate += direction;
            cratesPositions.Add(crate);
            CratesChange = true;

            Win = CheckCrateCondition(pointsPosition, cratesPositions);
        }

        public bool CheckCrateCondition(List<Vector2Int> points, List<Vector2Int> crates)
        {
            bool result = false;
            if (points != null)
            {
                foreach (Vector2Int point in points)
                {
                    if (!crates.Contains(point))
                    {
                        result = false;
                        break;
                    }

                    result = true;
                }
            }

            return result;
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

        public GridController(List<int> grid, Vector2Int playerPosition, List<Vector2Int> crates, Vector2Int gridSize)
        {
            InitGrid(grid);
            this.playerPosition = playerPosition;
            if (crates != null && crates.Count > 0)
                this.cratesPositions = new List<Vector2Int>(crates);
            this.GridSize = gridSize;
        }

        public List<Direction> GetActionFromPosition(Vector2Int pos)
        {
            List<Direction> dirs = new List<Direction>();
            foreach (Direction dir in (Direction[]) Enum.GetValues(typeof(Direction)))
            {
                if (CanStepOn(pos, dir))
                {
                    dirs.Add(dir);
                }
            }

            return dirs;
        }

        public static Vector2Int GetNextPosition(Vector2Int pos, Direction dir, out Vector2Int dirVec)
        {
            Vector2Int tmpDes = pos;
            dirVec = new Vector2Int();
            switch (dir)
            {
                case Direction.Up:
                    dirVec = Vector2Int.up;
                    break;
                case Direction.Down:
                    dirVec = Vector2Int.down;
                    break;
                case Direction.Right:
                    dirVec = Vector2Int.right;
                    break;
                case Direction.Left:
                    dirVec = Vector2Int.left;
                    break;
                default:
                    break;
            }

            return pos + dirVec;
        }

        public State GetState(List<State> states, Vector2Int playerPos, List<Vector2Int> cratePos)
        {
            return states.FirstOrDefault(state => state.PlayerInformation == playerPos && CompareTwoListOfVector2(cratePos, state.CratesInformation));
        }
        
        
        
        public State GetNextState(State currentState, Direction dir, List<State> possibleStates)
        {
            Vector2Int playerNextPos = GetNextPosition(currentState.PlayerInformation, dir, out Vector2Int dirVec);
            List<Vector2Int> cratesPos = null;
            if (currentState.CratesInformation != null)
                cratesPos = new List<Vector2Int>(currentState.CratesInformation);

            if (cratesPos != null && StepOnCrate(cratesPos, playerNextPos, out int index))
            {
                if (CanMoveCrate(playerNextPos, dirVec))
                    cratesPos[index] = GetNextPosition(playerNextPos, dir, out _);
            }

            foreach (State searchNextState in possibleStates)
            {
                if (searchNextState != null)
                {
                    if (searchNextState.PlayerInformation == playerNextPos)
                    {
                        if (cratesPos == null || CompareTwoListOfVector2(searchNextState.CratesInformation, cratesPos))
                        {
                            return searchNextState;
                        }
                    }
                }
            }

            return currentState;
        }

        public bool CompareTwoListOfVector2(List<Vector2Int> list1, List<Vector2Int> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }

            foreach (Vector2Int vec in list1)
            {
                if (!list2.Contains(vec))
                {
                    return false;
                }
            }

            return true;
        }

        public static float DirectionToAngle(Direction dir)
        {
            float angle = 0;
            switch (dir)
            {
                case Direction.Down:
                    angle = 180;
                    break;
                case Direction.Right:
                    angle = -90;
                    break;
                case Direction.Left:
                    angle = 90;
                    break;
                case Direction.Up:
                default:
                    angle = 0;
                    break;
            }

            return angle;
        }
    }
}