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
        public Vector2Int GridSize { get; protected set; }

        public List<Direction> Actions { get; protected set; } = new List<Direction>();
        public List<State> States { get; protected set; } = new List<State>();
        public Vector2Int PlayerStartPos { get; set; } = new Vector2Int();

        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        
        public void RunGrid(GridController grid, ReinforcementType type, int nbEpisodes)
        {
            Start = DateTime.Now;
            this.grid = grid;
            this.GridSize = grid.GridSize;
            this.PlayerStartPos = grid.PlayerPosition;
            List<State> possiblesStates = new List<State>();

            for (int y = 0; y < grid.GridSize.y; y++)
            {
                for (int x = 0; x < grid.GridSize.x; x++)
                {
                    Vector2Int playerPos = new Vector2Int(x, y);
                    switch ((GridType)grid.GetTile(playerPos))
                    {
                        case GridType.Door:
                            possiblesStates.Add(new State(GridType.Door, playerPos)
                            {
                                Reward = 1.0f
                            });
                            break;
                        case GridType.Ground:
                            possiblesStates.Add(
                                new State(GridType.Ground, playerPos, null,
                                    grid.GetActionFromPosition(new Vector2Int(x, y)))
                                {
                                    Reward = 0.0f
                                });
                            break;
                        case GridType.Hole:
                            possiblesStates.Add(
                                new State(GridType.Hole, playerPos, null,
                                    grid.GetActionFromPosition(new Vector2Int(x, y)))
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

            DynamicProgramming dp = null;
            switch (type)
            {
                case ReinforcementType.Policy:
                    dp = new PolicyIteration(possiblesStates, grid.GridSize.x);
                    dp.Evaluate(grid, 0.9f, 0.01f);
                    dp.Compute(grid.PlayerPosition);
                    break;
                // case ReinforcementType.Sarsra:
                //     break;
                // case ReinforcementType.McEs:
                //     break;
                // case ReinforcementType.McEsPolicyOn:
                //     break;
                case ReinforcementType.McEsPolicyOff:
                    dp = new MctsOff(possiblesStates, grid.GridSize.x, nbEpisodes);
                    dp.Evaluate(grid);
                    dp.Compute(grid.PlayerPosition);
                    break;

                case ReinforcementType.Value:
                default:
                    dp = new ValueIteration(possiblesStates, grid.GridSize.x);
                    dp.Evaluate(grid, 0.9f, 0.01f);
                    dp.Compute(grid.PlayerPosition);
                    break;
            }

            if (dp != null)
            {
                States = dp.states;
                Actions = dp.Actions;
            }
            End = DateTime.Now;
            Debug.Log(End-Start);
        }

        public void RunSokoban(GridController grid, ReinforcementType? type = null)
        {
            this.grid = grid;
            this.GridSize = grid.GridSize;
            List<State> possiblesStates = new List<State>();
            List<Vector2Int> possibleCratePos = new List<Vector2Int>();
            for (int y = 0; y < grid.GridSize.y; y++)
            {
                for (int x = 0; x < grid.GridSize.x; x++)
                {
                    Vector2Int playerPos = new Vector2Int(x, y);
                   
                    switch ((GridType)grid.GetTile(playerPos))
                    {
                        case GridType.Door:
                            possiblesStates.Add(new State(GridType.Door, playerPos)
                            {
                                Reward = 1.0f
                            });
                            break;
                        case GridType.Ground:
                            possiblesStates.Add(
                                new State(GridType.Ground, playerPos, null,
                                    grid.GetActionFromPosition(new Vector2Int(x, y)))
                                {
                                    Reward = 0.0f
                                });
                            break;
                        case GridType.Hole:
                            possiblesStates.Add(
                                new State(GridType.Hole, playerPos, null,
                                    grid.GetActionFromPosition(new Vector2Int(x, y)))
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

            DynamicProgramming dp = null;
            switch (type)
            {
                case ReinforcementType.Policy:
                    dp = new PolicyIteration(possiblesStates, grid.GridSize.x);
                    dp.Evaluate(grid, 0.9f, 0.01f);
                    dp.Compute(grid.PlayerPosition);
                    break;
                // case ReinforcementType.Sarsra:
                //     break;
                // case ReinforcementType.McEs:
                //     break;
                // case ReinforcementType.McEsPolicyOn:
                    break;
                case ReinforcementType.McEsPolicyOff:
                    break;

                case ReinforcementType.Value:
                default:
                    dp = new ValueIteration(possiblesStates, grid.GridSize.x);
                    dp.Evaluate(grid, 0.9f, 0.01f);
                    dp.Compute(grid.PlayerPosition);
                    break;
            }

            if (dp != null)
            {
                States = dp.states;
                Actions = dp.Actions;
            }
        }
    }
}