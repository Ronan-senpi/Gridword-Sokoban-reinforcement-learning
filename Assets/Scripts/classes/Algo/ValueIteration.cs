using System;
using System.Collections.Generic;
using UnityEngine;

namespace classes.Algo
{
    public class ValueIteration
    {
        public List<State?> states = new List<State?>();
        private Vector2Int gridSize;
        public float Gamma { get; private set; }

        public ValueIteration(GridController grid, float gamma, float theta = 0.01f)
        {
            #region Initialisation

            this.Gamma = gamma;
            float delta = 0f;
            List<int> gridValues = grid.StartGrid;
            this.gridSize = grid.GridSize;
            for (int y = 0; y < grid.GridSize.y; y++)
            {
                for (int x = 0; x < grid.GridSize.x; x++)
                {
                    switch ((GridType) grid.GetTile(new Vector2Int(x, y)))
                    {
                        case GridType.Door:
                            states.Add(new State(GridType.Door)
                            {
                                Reward = 1.0f
                            });
                            break;
                        case GridType.Ground:
                            states.Add(new State(GridType.Ground, grid.GetActionFromPosition(new Vector2Int(x, y)))
                            {
                                Reward = 0.0f
                            });
                            break;
                        case GridType.Hole:
                            states.Add(new State(GridType.Hole, grid.GetActionFromPosition(new Vector2Int(x, y)))
                            {
                                Reward = -1.0f
                            });
                            break;
                        case GridType.Void:
                        case GridType.Wall:
                        case GridType.Point:
                        case GridType.Crate:
                        case GridType.Arrow:
                        case GridType.Character:
                        default:
                            states.Add(null);
                            break;
                    }
                }
            }

            #endregion

            int maxLoop = 0;
            do
            {
                maxLoop++;
                delta = 0;
                //Pourquoi initialisé avant ? pour libiré de la puissance de calcule dans le do while
                for (int y = 0; y < grid.GridSize.y; y++)
                {
                    for (int x = 0; x < grid.GridSize.x; x++)
                    {
                        int stateIndex = y * grid.GridSize.x + x;
                        if (states[stateIndex] != null)
                        {
                            State currentPosition = states[stateIndex].Value;
                            float actionValue = 0;
                            if (currentPosition.Actions != null)
                            {
                                foreach (Direction action in currentPosition.Actions)
                                {
                                    Vector2Int nextPos = GridController.GetNextPosition(new Vector2Int(x, y), action);
                                    int nextIndex = nextPos.y * grid.GridSize.x + nextPos.x;
                                    if (states[nextIndex].HasValue)
                                    {
                                        float tmpStateValue = states[nextIndex].Value.GetReward() +
                                                              gamma * states[nextIndex].Value.StateValue;
                                        if (actionValue < tmpStateValue)
                                        {
                                            actionValue = tmpStateValue;
                                            currentPosition.BestAction = action;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                actionValue = currentPosition.GetReward();
                            }

                            delta = Mathf.Max(delta, Mathf.Abs(currentPosition.StateValue - actionValue));
                            currentPosition.StateValue = actionValue;
                        }
                    }
                }

                if (maxLoop >= 10000)
                {
                    break;
                }
            } while (delta < theta);
        }

        public List<Direction> Compute(Vector2Int playerPos)
        {
            List<Direction> actions = new List<Direction>();
            int playerIndex;
            int loopCount = 0;
            do
            {
                playerIndex = playerPos.y * gridSize.x + playerPos.x;
                if (states[playerIndex] != null)
                {
                    State current = states[playerIndex].Value;
                    
                    if (GridType.Door == current.CellType)
                        break;

                    actions.Add(current.BestAction);
                    playerPos = GridController.GetNextPosition(playerPos, current.BestAction);
                }

                ++loopCount;
            } while (loopCount < 10000);
            return actions;
        }

        public struct GameState
        {
            public Vector2Int PlayerPos { get; set; }
            public List<Vector2Int> CratesPos { get; set; }
        }

        public struct State
        {
            public GridType CellType { get; private set; }
            public float Reward { get; set; }
            public float StateValue { get; set; }
            public List<Direction> Actions { get; private set; }

            public Direction BestAction { get; set; }

            public State(GridType type, List<Direction> actions = null)
            {
                CellType = type;
                StateValue = 0;
                Actions = actions;
                Reward = 0;
                BestAction = Direction.Up;
            }

            public float GetReward()
            {
                return this.Reward;
            }
        }
    }
}