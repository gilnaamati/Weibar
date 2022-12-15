using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CellGrid : MonoBehaviour
{
    public int width, length;
    public GameObject cellPrefab;

    public List<Cell> cellList = new List<Cell>();
    
    void PopulateGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                var c = PrefabUtility.InstantiatePrefab(cellPrefab, transform).GetComponent<Cell>();
                c.xPos = i;
                c.yPos = j;
                cellList.Add(c);
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
