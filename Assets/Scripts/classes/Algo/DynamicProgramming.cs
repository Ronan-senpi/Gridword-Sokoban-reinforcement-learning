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
        public List<State> states { get; protected set; } = new List<State>();
        protected int gridWidth;
        public List<Direction> Actions { get; set; } 

        public abstract void Evaluate(GridController grid, float gamma = 0.9f, float theta = 0.01f);
        
        public void Compute(Vector2Int playerPos)
        {

            Actions = new List<Direction>();
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
                        Actions.Add(current.BestAction);
                        playerPos = GridController.GetNextPosition(playerPos, current.BestAction);
                    }
                }

                ++loopCount;
            } while (loopCount < 10000);
        }        
    }
}