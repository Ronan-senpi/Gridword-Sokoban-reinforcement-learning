using System.Collections.Generic;
using classes;
using UnityEngine;

public class State
{
    public GridType CellType { get; private set; }
    public Vector2Int PlayerInformation { get; private set; }
    public List<Vector2Int> CratesInformation { get; private set; }
    
    public float Reward { get; set; }
    public float StateValue { get; set; }
    public List<Direction> Actions { get; private set; }

    public Direction BestAction { get; set; }

    public State(GridType type, Vector2Int playerPos, List<Vector2Int> cratesPos = null, List<Direction> actions = null)
    {
        CellType = type;
        PlayerInformation = playerPos;
        CratesInformation = cratesPos;
        StateValue = 0;
        Actions = actions;
        Reward = 0;
        BestAction = Direction.Down;
    }
    
    public float GetReward()
    {
        return this.Reward;
    }

    public bool Victory(GridController grid)
    {
        if (GameType.Instance.IsSokoban)
        {
            return grid.CompareTwoListOfVector2(grid.pointsPosition, CratesInformation);
        }
        else
        {
            return CellType == GridType.Door;

        }
    }
}