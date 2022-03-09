using System.Collections.Generic;
using classes;

public class State
{
    public GridType CellType { get; private set; }
    public float Reward { get; set; }
    public float StateValue { get; set; }
    public List<Direction> Actions { get; private set; }

    public Direction BestAction { get; set; }

    public State(GridType type, List<Direction> actions = null)
    {
        CellType = type;
        StateValue = 0;
        Actions = actions;
        Reward = 0;
        BestAction = Direction.Down;
    }

    public float GetReward()
    {
        return this.Reward;
    }
}