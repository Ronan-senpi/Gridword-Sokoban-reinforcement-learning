using System.Collections;
using System.Collections.Generic;
using classes;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [Range(10,999)]
    [SerializeField]  private int nbTry = 10;

    public Stats GenerateStats(int lvlIndex)
    {

        Stats s = new Stats(lvlIndex);
        for (int i = 0; i < nbTry; i++)
        {
            AIManager.Instance.AIStart();
            s.AddTime(AIManager.Instance.GetExecuteTime());
        }

        return s;
    }

}
