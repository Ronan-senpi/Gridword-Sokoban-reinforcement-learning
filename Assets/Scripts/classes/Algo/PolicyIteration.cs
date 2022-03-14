using System.Collections.Generic;
using UnityEngine;

namespace classes.Algo
{
    public class PolicyIteration : DynamicProgramming
    {
        public PolicyIteration(List<State> possiblesStates, int width)
        {
            states = possiblesStates;
            gridWidth = width;
            foreach (State state in states)
            {
                if (state != null)
                {
                    int random = Random.Range(0, 4);
                    state.BestAction = (Direction)random;
                }
            }
        }

        public override void Evaluate(GridController grid, float gamma = 0.9f, float theta = 0.01f)
        {
            this.grid = grid;
            int maxLoop = 0;
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
                        if (state.BestAction != null)
                        {
                            State nextState = grid.GetNextState(state, state.BestAction, states);
                            if (nextState != null)
                            {
                                float tmpStateValue = nextState.GetReward() +
                                                      gamma * nextState.StateValue;
                                if (actionValue < tmpStateValue)
                                {
                                    actionValue = tmpStateValue;
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

                if (maxLoop >= 100000)
                {
                    break;
                }
            } while (delta > theta);


            bool stable = true;
            foreach (State state in states)
            {
                if (state != null)
                {
                    Direction temp = state.BestAction;
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

                    if (state.BestAction != temp)
                    {
                        stable = false;
                    }
                }
            }


            if (stable)
            {
                return;
            }
            else
            {
                Evaluate(grid, gamma, theta);
            }
        }
    }
}