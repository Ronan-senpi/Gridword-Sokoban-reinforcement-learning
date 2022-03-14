using System;
using System.Collections;
using System.Collections.Generic;
using classes;
using TMPro;
using UnityEngine;


public class AIManager : MonoBehaviour
{
    public float ActionDelay { get; set; } = 0.1f;

    private AIController ac = new AIController();


    [Header("AI settings")] [SerializeField]
    private ReinforcementType reinforcementType = ReinforcementType.Value;

    public ReinforcementType ReinforcementType => reinforcementType;
    [ReadOnly] [SerializeField] protected bool computeIsOver = false;

    public bool ComputeIsOver
    {
        get { return computeIsOver; }
        set { computeIsOver = value; }
    }

    [SerializeField] protected GameObject arrowPrefab;
    public static AIManager Instance { get; set; }
    public int NbEpisode { get; set; } = 1;


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
            yield return new WaitForSeconds(ActionDelay);
        }
    }

    public void AIStart()
    {

        computeIsOver = false;
        if (GameType.Instance.IsSokoban)
        {
            ac.RunSokoban(GridManager.Instance.gc, reinforcementType);
        }
        else
        {
            ac.RunGrid(GridManager.Instance.gc, reinforcementType, NbEpisode);
        }

        computeIsOver = true;
    }


    public void PlayActions()
    {
        MovePlayer(ac.Actions);
    }

    public TimeSpan GetExecuteTime()
    {
        return ac.Span.Value;
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
                        text.text = Math.Round(s.StateValue, 2).ToString();
                    }
                }
            }
        }
    }

    public void ClearChild()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void DisplayAllArrows()
    {
        ClearChild();
        List<State> states = ac.States;
        State s;
        for (int y = 0; y < ac.GridSize.y; y++)
        {
            for (int x = 0; x < ac.GridSize.x; x++)
            {
                s = states[y * ac.GridSize.x + x];
                if (s != null)
                {
                    InstantiateArrow(x, y, s.BestAction);
                }
            }
        }
    }

    private Vector2Int GetDirVector(Direction dir)
    {
        Vector2Int vec = Vector2Int.zero;
        switch (dir)
        {
            case Direction.Up:
                vec += Vector2Int.up;
                break;
            case Direction.Down:
                vec += Vector2Int.down;
                break;
            case Direction.Right:
                vec += Vector2Int.right;
                break;
            case Direction.Left:
                vec += Vector2Int.left;
                break;
        }

        return vec;
    }

    private void InstantiateArrow(int x, int y, Direction dir)
    {
        float ArrowRotZ = GridController.DirectionToAngle(dir);
        GameObject go = Instantiate(arrowPrefab, new Vector3(x, y), Quaternion.identity, transform);
        go.transform.Rotate(0, 0, ArrowRotZ);
    }

    public void DisplayCriticPath()
    {
        if (ac.Actions == null || ac.Actions.Count == 0)
        {
            Debug.LogWarning("ac.Actions is empty");
            return;
        }

        ClearChild();
        List<State> states = ac.States;
        Vector2Int vec = ac.PlayerStartPos;
        foreach (var direction in ac.Actions)
        {
            ;
            vec += GetDirVector(direction);
            State s = states[vec.y * ac.GridSize.x + vec.x];
            InstantiateArrow(vec.x, vec.y, s.BestAction);
        }
    }
}