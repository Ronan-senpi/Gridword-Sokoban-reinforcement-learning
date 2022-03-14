using System;
using System.Collections;
using System.Collections.Generic;
using classes.Algo;
using UnityEditor;
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
        public TimeSpan? Span { get; set; }

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
            Span = End - Start;
        }

        public void RunSokoban(GridController grid, ReinforcementType? type = null)
        {
            this.Start = DateTime.Now;
            this.grid = grid;
            this.GridSize = grid.GridSize;
            List<State> possiblesStates = new List<State>();
            for (int y = 0; y < grid.GridSize.y; y++)
            {
                for (int x = 0; x < grid.GridSize.x; x++)
                {
                    Vector2Int playerPos = new Vector2Int(x, y);
                    if ((GridType)grid.GetTile(playerPos) != GridType.Wall)
                    {
                        List<Vector2Int> cratePos = new List<Vector2Int>();
                        State newState = new State(GridType.Ground, playerPos, cratePos,
                            grid.GetActionFromPosition(playerPos));
                        possiblesStates.Add(newState);
                    }
                }
            }

            possiblesStates = GenerateCrateStates(possiblesStates, grid, grid.CratesPositions.Count);
            DynamicProgramming dp = null;
            switch (type)
            {
                case ReinforcementType.Policy:
                    dp = new PolicyIteration(possiblesStates, grid.GridSize.x);
                    dp.Evaluate(grid, 0.9f, 0.01f);
                    dp.Compute(grid.PlayerPosition, grid.CratesPositions);
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
                    dp.Compute(grid.PlayerPosition, grid.CratesPositions);
                    break;
            }

            if (dp != null)
            {
                States = dp.states;
                Actions = dp.Actions;
            }

            this.End = DateTime.Now;
            this.Span = End - Start;
        }

        public List<State> GenerateCrateStates(List<State> currentStates, GridController grid, int crateNeeded)
        {
            if (crateNeeded <= 0) return currentStates;

            bool crateFound = false;
            List<State> possibleCrateStates = new List<State>();
            foreach (State state in currentStates)
            {
                for (int y = 0; y < grid.GridSize.y; y++)
                {
                    for (int x = 0; x < grid.GridSize.x; x++)
                    {
                        Vector2Int currentPos = new Vector2Int(x, y);
                        GridType type = (GridType)grid.GetTile(currentPos);
                        if (type != GridType.Wall && state.PlayerInformation != currentPos)
                        {
                            crateFound = false;
                            foreach (Vector2Int crateInfo in state.CratesInformation)
                            {
                                if (crateInfo == currentPos)
                                {
                                    crateFound = true;
                                    break;
                                }
                            }

                            if (!crateFound)
                            {
                                List<Vector2Int> copyCrate = new List<Vector2Int>(state.CratesInformation);
                                copyCrate.Add(currentPos);
                                State newState = new State(type, state.PlayerInformation, copyCrate,
                                    grid.GetActionFromPosition(state.PlayerInformation));

                                if (grid.CheckCrateCondition(copyCrate))
                                {
                                    newState.Reward = 1;
                                }
                                else
                                {
                                    newState.Reward = 0;
                                }

                                possibleCrateStates.Add(newState);
                            }
                        }
                    }
                }
            }

            return GenerateCrateStates(possibleCrateStates, grid, crateNeeded - 1);
        }
    }
}