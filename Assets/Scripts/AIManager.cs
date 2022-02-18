using System.Collections;
using System.Collections.Generic;
using classes;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public void AIStart()
    {
        AIController ac = new AIController(GridManager.Instance.gc);
    }
}
