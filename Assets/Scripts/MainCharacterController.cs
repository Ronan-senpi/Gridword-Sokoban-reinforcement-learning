using System;
using System.Collections;
using System.Collections.Generic;
using classes;
using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    /// <summary>
    /// Move player on direction 
    /// </summary>
    /// <param name="direction">
    /// for every axes
    /// Max 1
    /// Min -1
    /// </param>
    public void Move(Vector2Int direction)
    {
        Vector2Int pos = new Vector2Int((int)transform.position.x,(int)transform.position.y);
        GridManager.Instance.gc.MovePlayer(direction);
       //  Vector3 vec = CapVector(direction);
       //  int gt;
       //  bool canStep;
       // (canStep, gt) = GridManager.Instance.canSteoOn(transform.position + vec, vec);
       //  if(!canStep)    
       //      return;
        // transform.position += vec;
    }


    private Vector3 CapVector(Vector2Int value)
    {
        int min = -1;
        int max = 1;
        return new Vector2((value.x < min) ? min : (value.x> max) ? max : value.x,
                                (value.y < min) ? min : (value.y> max) ? max : value.y);
    }
}
