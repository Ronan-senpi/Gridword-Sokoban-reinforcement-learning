﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace classes.Algo
{
    public class ValueIteration : DynamicProgramming
    {
        private int gridWidth;
        public ValueIteration(List<State> possiblesStates, int width)
        {
            this.states = possiblesStates;
            this.gridWidth = width;
        }
        
        public override void Evaluate(GridController grid, float gamma = 0.9f, float theta = 0.01f)
        {
            delta = 0f;
            maxLoop = 0;
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
                            float temp = states[stateIndex].StateValue;
                            float actionValue = 0;
                            if (states[stateIndex].Actions != null)
                            {
                                foreach (Direction action in states[stateIndex].Actions)
                                {
                                    Vector2Int nextPos = GridController.GetNextPosition(new Vector2Int(x, y), action);
                                    int nextIndex = nextPos.y * grid.GridSize.x + nextPos.x;
                                    if (states[nextIndex] != null)
                                    {
                                        float tmpStateValue = states[nextIndex].GetReward() +
                                                              gamma * states[nextIndex].StateValue;
                                        if (actionValue < tmpStateValue)
                                        {
                                            actionValue = tmpStateValue;
                                            states[stateIndex].BestAction = action;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                actionValue = states[stateIndex].GetReward();
                            }

                            states[stateIndex].StateValue = actionValue;
                            delta = Mathf.Max(delta, Mathf.Abs(temp - actionValue));
                        }
                    }
                }

                if (maxLoop >= 10000)
                {
                    break;
                }
            } while (delta > theta);
        }

        public override List<Direction> Compute(Vector2Int playerPos)
        {
            List<Direction> actions = new List<Direction>();
            int playerIndex;
            int loopCount = 0;
            do
            {
                playerIndex = playerPos.y * gridWidth + playerPos.x;
                if (states[playerIndex] != null)
                {
                    State current = states[playerIndex];

                    if (GridType.Door == current.CellType)
                        break;
                    if (current.BestAction != null)
                    {
                        actions.Add(current.BestAction);
                        playerPos = GridController.GetNextPosition(playerPos, current.BestAction);
                    }
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
    }
}