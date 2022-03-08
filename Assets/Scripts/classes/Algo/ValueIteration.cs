using System;
using System.Collections.Generic;
using UnityEngine;

namespace classes.Algo
{
    public class ValueIteration
    {
        public List<State?> states = new List<State?>();

        public float Gamma { get; private set; }

        public ValueIteration(GridController grid, float gamma, float theta = 0.01f)
        {
            #region Initialisation

            this.Gamma = gamma;
            float delta = 0f;
            List<int> gridValues = grid.StartGrid;
            for (int y = 0; y < grid.GridSize.y; y++)
            {
                for (int x = 0; x < grid.GridSize.x; x++)
                {
                    switch ((GridType) grid.GetTile(new Vector2Int(x, y)))
                    {
                        case GridType.Door:
                            states.Add(new State()
                            {
                                Reward = 1.0f
                            });
                            break;
                        case GridType.Ground:
                            states.Add(new State(grid.GetActionFromPosition(new Vector2Int(x, y)))
                            {
                                Reward = 0.0f
                            });
                            break;
                        case GridType.Hole:
                            states.Add(new State(grid.GetActionFromPosition(new Vector2Int(x, y)))
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
                                    Vector2Int nextPos = grid.GetNextPosition(new Vector2Int(x, y), action);
                                    int nextIndex = nextPos.y * grid.GridSize.x + nextPos.x;
                                    if (states[nextIndex].HasValue)
                                    {
                                        float tmpStateValue = currentPosition.GetReward() +
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

        public List<State> Compute()
        {
            return null;
        }
    }

    public struct GameState
    {
        public Vector2Int PlayerPos { get; set; }
        public List<Vector2Int> CratesPos { get; set; }
    }

    public struct State
    {
        public float Reward { get; set; }
        public float StateValue { get; set; }
        public List<Direction> Actions { get; private set; }

        public Direction BestAction { get; set; }

        public State(List<Direction> actions = null)
        {
            StateValue = 0;
            Actions = actions;
            Reward = 0;
            BestAction = Direction.Down;
        }

        public float GetReward()
        {
            return this.Reward;
        }
    }
}