using System.Collections.Generic;
using UnityEngine;

namespace classes.Algo
{
    public class MctsOff : DynamicProgramming
    {
        private int nbEpisode;
        public MctsOff(List<State> possiblesStates, int width, int nbEpisode)
        {
            states = possiblesStates;
            gridWidth = width;
            this.nbEpisode = nbEpisode;
        }
        public override void Evaluate(GridController grid, float gamma = 0.9f, float theta = 0.01f)
        {

            for (int i = 0; i < nbEpisode; i++)
            {
                
            }
            
            
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
                                State nextState = GridController.GetNextState(state, action, states);
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
    }
}