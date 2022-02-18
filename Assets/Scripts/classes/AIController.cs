﻿using System;
using classes.Algo;

namespace classes
{
    public class AIController
    {
        private GridController grid;
        public AIController(GridController grid, ReinforcementType? type =null)
        {
            this.grid = grid;
            switch (type)
            {
                case ReinforcementType.Policy:
                    break;
                case ReinforcementType.Sarsra:
                    break;
                case ReinforcementType.MctsEs:
                    break;
                case ReinforcementType.MctsOn:
                    break;
                case ReinforcementType.MctsOff:
                    break;
                
                case ReinforcementType.Value:
                default:
                    ValueIteration vi = new ValueIteration(0.9f, grid);
                    vi.Compute();
                    break;
            }
        }
        
    }
}