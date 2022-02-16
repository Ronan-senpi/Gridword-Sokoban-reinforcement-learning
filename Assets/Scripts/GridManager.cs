using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    #region Members

    [Header("Grid Setup")]
    //size of a case (width & height)
    [SerializeField]
    private Vector2 caseDimensions;

    //number of case in grid (width & height)
    [SerializeField] private Vector2 gridDimensions;
    [SerializeField] private List<GameObject> ground;

    #endregion

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int i = 0; i < gridDimensions[0]; i++)
        {
            for (int j = 0; j < gridDimensions[1]; j++)
            {
                Vector3 casePosition = transform.position;
                casePosition.x += i * 100 - 60;
                casePosition.y += j * 100;
                GameObject currentGround = Instantiate(ground[0], casePosition, Quaternion.identity, transform);
                currentGround.name = "Case " + (i + j);
            }
        }
    }
}