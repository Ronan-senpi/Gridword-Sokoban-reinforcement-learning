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

        public abstract void Evaluate(GridController grid, float gamma = 0.9f, float theta = 0.01f);
        public abstract List<Direction> Compute(Vector2Int playerPos);
        
    }
}