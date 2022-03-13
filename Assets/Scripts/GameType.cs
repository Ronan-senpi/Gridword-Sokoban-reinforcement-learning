using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameType : MonoBehaviour
{
    [SerializeField] private bool isSokoban = false;
    [SerializeField] private TMP_Text gameTitle;
    public static GameType Instance { get; set; }

    public bool IsSokoban
    {
        get
        {
            return isSokoban;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (IsSokoban)
        {
            gameTitle.text = "Sokoban";
        }
        else
        {
            gameTitle.text = "Gird World";
        }
    }
}
