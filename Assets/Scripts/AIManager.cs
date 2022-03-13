using System;
using System.Collections;
using System.Collections.Generic;
using classes;
using TMPro;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [Range(0, 4)] [SerializeField] private float actionDelay = 0.1f;
    private AIController ac = new AIController();
    [SerializeField] private ReinforcementType reinforcementType = ReinforcementType.Policy;
    public static AIManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void MovePlayer(List<Direction> actions)
    {
        StartCoroutine(MovePlayerLoop(actions));
    }

    private IEnumerator MovePlayerLoop(List<Direction> actions)
    {
        foreach (var direction in actions)
        {
            Vector2Int vec = Vector2Int.zero;
            switch (direction)
            {
                case Direction.Up:
                    vec = Vector2Int.up;
                    break;
                case Direction.Down:
                    vec = Vector2Int.down;
                    break;
                case Direction.Right:
                    vec = Vector2Int.right;
                    break;
                case Direction.Left:
                    vec = Vector2Int.left;
                    break;
                // default:
                //     throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            GridManager.Instance.gc.MovePlayer(vec);
            yield return new WaitForSeconds(actionDelay);
        }
    }
    public void AIStart()
    {
        ac.RunSokoban(GridManager.Instance.gc, reinforcementType);
    }

    public void PlayActions()
    {
        MovePlayer(ac.Actions);
    }
    
    public void DisplayStateValue()
    {
        List<State> states = ac.States;
        State s;
        for (int y = 0; y < ac.GridSize.y; y++)
        {
            for (int x = 0; x < ac.GridSize.x; x++)
            {
                s = states[y * ac.GridSize.x + x];
                if (s != null)
                {
                    TMP_Text text;
                    if (GridManager.Instance.TryGetTextCell(x, y, out text))
                    {
                        text.text = Math.Round(s.StateValue,2).ToString();
                    }
                }
            }
        }
    }

}