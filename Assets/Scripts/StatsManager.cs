using System.Collections;
using System.Collections.Generic;
using classes;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [Range(10,999)]
    [SerializeField]  private int nbTry = 10;

    public void GenerateStats()
    {
        if (!GridManager.Instance.LevelLoaded)
        {
            Debug.LogWarning("Load level before start stats");
            return;
        }
        
        Stats s = new Stats(LevelManager.Instance.GetLevelIndex());
        for (int i = 0; i < nbTry; i++)
        {
            AIManager.Instance.AIStart();
            s.AddTime(AIManager.Instance.GetExecuteTime());
        }
       // s.Write();
        s.ToJson();
    }

}
