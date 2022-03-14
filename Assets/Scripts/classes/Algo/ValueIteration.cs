using System;
using System.Collections.Generic;
using UnityEngine;

namespace classes.Algo
{
    public class ValueIteration : DynamicProgramming
    {
        public ValueIteration(List<State> possiblesStates, int width)
        {
            states = possiblesStates;
            gridWidth = width;
        }

        public override void Evaluate(GridController grid, float gamma = 0.9f, float theta = 0.01f)
        {
            this.grid = grid;
            delta = 0f;
            maxLoop = 0;
            do
            {
                maxLoop++;
                delta = 0;
                //Pourquoi initialisé avant ? pour libiré de la puissance de calcule dans le do while
                foreach (State state in states)
                {
                    if (state != null)
                    {
                        float temp = state.StateValue;
                        float actionValue = 0;
                        if (state.Actions != null)
                        {
                            foreach (Direction action in state.Actions)
                            {
                                State nextState = grid.GetNextState(state, action, states);
                                if (nextState != null)
                                {
                                    float tmpStateValue = nextState.GetReward() +
                                                          gamma * nextState.StateValue;
                                    if (actionValue < tmpStateValue)
                                    {
                                        actionValue = tmpStateValue;
                                        state.BestAction = action;
                                    }
                                }
                            }
                        }
                        else
                        {
                            actionValue = state.GetReward();
                        }

                        state.StateValue = actionValue;
                        delta = Mathf.Max(delta, Mathf.Abs(temp - actionValue));
                    }
                }


                if (maxLoop >= 10000)
                {
                    break;
                }
            } while (delta > theta);
        }


        public struct GameState
        {
            public Vector2Int PlayerPos { get; set; }
            public List<Vector2Int> CratesPos { get; set; }
        }
    }
}