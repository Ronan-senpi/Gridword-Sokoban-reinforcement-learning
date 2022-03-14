using System.Collections;
using System.Collections.Generic;
using classes;
using classes.Algo;
using UnityEngine;

namespace classes.Algo
{
    public abstract class DynamicProgramming
    {
        protected float delta;
        protected uint maxLoop;
        public List<State> states { get; protected set; } = new List<State>();
        protected int gridWidth;
        public List<Direction> Actions { get; set; }

        protected GridController grid;

        public abstract void Evaluate(GridController grid, float gamma = 0.9f, float theta = 0.01f);

       
        public void Compute(Vector2Int playerPos, List<Vector2Int> cratePos = null)
        {
            Actions = new List<Direction>();
            State stateToCheck;
            List<Vector2Int> crateCopy = null;
            if (cratePos != null)
            {
                crateCopy = new List<Vector2Int>(cratePos);
            }

            stateToCheck = grid.GetState(states, playerPos, crateCopy);


            int loopCount = 0;
            do
            {
                if (stateToCheck != null)
                {
                    State current = stateToCheck;

                    if (current.Victory(grid))
                        break;
                    if (current.BestAction != null)
                    {
                        Actions.Add(current.BestAction);
                        playerPos = GridController.GetNextPosition(playerPos, current.BestAction, out var dirVec);
                        if (crateCopy != null && GridController.StepOnCrate(crateCopy, playerPos, out int index))
                        {
                            if (grid.CanMoveCrate(playerPos, dirVec))
                                crateCopy[index] = GridController.GetNextPosition(playerPos, current.BestAction, out _);
                        }
                    }

                    stateToCheck = grid.GetState(states, playerPos, crateCopy);
                }

                ++loopCount;
            } while (loopCount < 10000);
        }
    }
}