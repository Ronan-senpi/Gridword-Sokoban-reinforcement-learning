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
        protected  uint maxLoop;
        protected List<State> states = new List<State>();
        protected int gridWidth;

        public abstract void Evaluate(GridController grid, float gamma = 0.9f, float theta = 0.01f);
        
        public List<Direction> Compute(Vector2Int playerPos)
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
    }
}