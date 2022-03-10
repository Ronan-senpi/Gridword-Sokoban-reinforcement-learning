using System;
using System.Collections;
using System.Collections.Generic;
using classes.Algo;
using UnityEngine;

namespace classes
{
    public class AIController
    {
        private GridController grid;

        public AIController(GridController grid, ReinforcementType? type = null)
        {
            this.grid = grid;
            List<State> possiblesStates = new List<State>();

            for (int y = 0; y < grid.GridSize.y; y++)
            {
                for (int x = 0; x < grid.GridSize.x; x++)
                {
                    switch ((GridType)grid.GetTile(new Vector2Int(x, y)))
                    {
                        case GridType.Door:
                            possiblesStates.Add(new State(GridType.Door, null)
                            {
                                Reward = 1.0f
                            });
                            break;
                        case GridType.Ground:
                            possiblesStates.Add(
                                new State(GridType.Ground, grid.GetActionFromPosition(new Vector2Int(x, y)))
                                {
                                    Reward = 0.0f
                                });
                            break;
                        case GridType.Hole:
                            possiblesStates.Add(
                                new State(GridType.Hole, grid.GetActionFromPosition(new Vector2Int(x, y)))
                                {
                                    Reward = -15f
                                });
                            break;
                        case GridType.Void:
                        case GridType.Wall:
                        case GridType.Point:
                        case GridType.Crate:
                        case GridType.Arrow:
                        case GridType.Character:
                        default:
                            possiblesStates.Add(null);
                            break;
                    }
                }
            }

            List<Direction> actions = new List<Direction>();
            switch (type)
            {
                case ReinforcementType.Policy:
                    PolicyIteration pi = new PolicyIteration(possiblesStates, grid.GridSize.x);
                    pi.Evaluate(grid, 0.9f, 0.01f);
                    actions = pi.Compute(grid.PlayerPosition);

                    break;
                case ReinforcementType.Sarsra:
                    break;
                case ReinforcementType.MctsEs:
                    break;
                case ReinforcementType.MctsOn:
                    break;
                case ReinforcementType.MctsOff:
                    break;

                case ReinforcementType.Value:
                default:
                    ValueIteration vi = new ValueIteration(possiblesStates, grid.GridSize.x);
                    vi.Evaluate(grid, 0.9f, 0.01f);
                    actions = vi.Compute(grid.PlayerPosition);
                    break;
            }

            AIManager.Instance.MovePlayer(actions);
        }
    }
}