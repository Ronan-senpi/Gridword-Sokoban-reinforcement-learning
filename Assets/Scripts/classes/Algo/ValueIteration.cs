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
                    switch ((GridType)grid.GetTile(new Vector2Int(x, y)))
                    {
                        case GridType.Door:
                            states.Add(new State(1));
                            break;
                        case GridType.Ground:
                            states.Add(new State(0, grid.GetActionFromPosition(new Vector2Int(x, y))));
                            break;
                        case GridType.Hole:
                            states.Add(new State(-1, grid.GetActionFromPosition(new Vector2Int(x, y))));
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

            do
            {
                delta = 0;
                for (int y = 0; y < grid.GridSize.y; y++)
                {
                    for (int x = 0; x < grid.GridSize.x; x++)
                    {
                        int stateIndex = y * grid.GridSize.x + x;
                        if (states[stateIndex] != null)
                        {
                            float temp = states[stateIndex].StateValue;
                            float actionValue = 0;
                            foreach (Direction action in states[stateIndex].Actions)
                            {
                                Vector2Int nextState = grid.GetNextPosition(new Vector2Int(x, y), action);
                                actionValue = states[nextState.y * grid.GridSize.x + nextState.x].GetReward() + gamma;
                            }
                        }
                    }
                }
            } while (delta < theta);
        }

        public List<State> Compute()
        {
            return null;
        }
    }

    public class State
    {
        public State(float stateValue, List<Direction> actions = null)
        {
            StateValue = stateValue;
            Actions = actions;
        }

        public float StateValue { get; set; }
        public List<Direction> Actions { get; private set; }

        public float GetReward()
        {
            return 0.0f;
        }
    }
}