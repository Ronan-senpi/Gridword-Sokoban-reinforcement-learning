using System;
using System.Collections;
using System.Collections.Generic;
using classes;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [Range(0, 4)] [SerializeField] private float actionDelay = 1f;

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

    public void AIStart()
    {
        AIController ac = new AIController(GridManager.Instance.gc, ReinforcementType.Value);
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            GridManager.Instance.gc.MovePlayer(vec);
            yield return new WaitForSeconds(actionDelay);
        }
    }
}