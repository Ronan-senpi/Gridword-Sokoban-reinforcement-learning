using System;
using System.Collections.Generic;
using UnityEngine;

namespace classes.Algo
{
    public class ValueIteration
    {
        public List<State?> states = new List<State?>();

        public float Gamma { get; private set; }

        public ValueIteration(float gamma, GridController grid)
        {
            this.Gamma = gamma;
            List<int> gridValues = grid.StartGrid;
            for (int y = 0; y < grid.GridSize.y; y++)
            {
                for (int x = 0; x < grid.GridSize.x; x++)
                {
                    switch ((GridType) grid.GetTile(new Vector2Int(x, y)))
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
        }

        public List<State> Compute()
        {
            return null;
        }
    }

    public struct State
    {
        public State(float? value, List<Direction> actions = null)
        {
            Value = value;
            Actions = actions;
        }

        public float? Value { get; set; }
        private List<Direction> Actions { get; set; }

        public float GetReward()
        {
            return 0.0f;
        }
    }
}