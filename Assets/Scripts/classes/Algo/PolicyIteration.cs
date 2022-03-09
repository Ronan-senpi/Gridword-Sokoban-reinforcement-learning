using System.Collections.Generic;
using UnityEngine;

namespace classes.Algo
{
    public class PolicyIteration : DynamicProgramming
    {
        public PolicyIteration(List<State> possiblesStates)
        {
            states = possiblesStates;
            foreach (State state in states)
            {
                state.BestAction = (Direction)Random.Range(0, 4);
            }
        }

        public override void Evaluate(GridController grid, float gamma = 0.9f, float theta = 0.01f)
        {
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
                            float temp = states[stateIndex].StateValue;
                            float actionValue = 0;
                            if (states[stateIndex].BestAction != null)
                            {
                                Vector2Int nextPos = GridController.GetNextPosition(new Vector2Int(x, y), states[stateIndex].BestAction);
                                int nextIndex = nextPos.y * grid.GridSize.x + nextPos.x;
                                if (states[nextIndex] != null)
                                {
                                    float tmpStateValue = states[nextIndex].GetReward() +
                                                          gamma * states[nextIndex].StateValue;
                                    if (actionValue < tmpStateValue)
                                    {
                                        actionValue = tmpStateValue;
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

                if (maxLoop >= 100000)
                {
                    break;
                }
            } while (delta > theta);

            bool stable = true;
            foreach (State state in states)
            {
                Direction temp = state.BestAction;
                // state.BestAction = un truc
                if (temp != state.BestAction)
                {
                    stable = false;
                }
            }

            if (stable)
            {
                return;
            }

            Evaluate(grid, gamma, theta);
        }


        public override List<Direction> Compute(Vector2Int playerPos)
        {
            throw new System.NotImplementedException();
        }
    }
}