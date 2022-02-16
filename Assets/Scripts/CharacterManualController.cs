using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CharacterManualController : MainCharacterController
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.Move(Vector2Int.up);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.Move(Vector2Int.left);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.Move(Vector2Int.right);
        }
    }
}