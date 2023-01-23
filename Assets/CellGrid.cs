using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
[SelectionBase]
public class CellGrid : MonoBehaviour
{
    public int width, length;
    public GameObject cellPrefab;

    public List<Cell> cellList = new List<Cell>();

    [FormerlySerializedAs("cellHolder")] public Transform collider;
    public Transform holder;
    
    private void Awake()
    {
        PopulateGrid();
    }

    public void PopulateGrid()
    {
        var l = holder.GetComponentsInChildren<Cell>().ToList();
  
       foreach (var v in l) DestroyImmediate(v.gameObject);
        
        cellList.Clear();
        
        collider.transform.localPosition = new Vector3(0,- 0.5f * collider.transform.localScale.y, 0);
        var cellScale = new Vector3(collider.transform.localScale.x / (float)width, collider.transform.localScale.y,
            collider.transform.localScale.z / (float)length);

        var startPos = new Vector3(-collider.transform.localScale.x * 0.5f, 0.5f * collider.transform.localScale.y,
            -collider.transform.localScale.z * 0.5f);
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                var c = PrefabUtility.InstantiatePrefab(cellPrefab, holder).GetComponent<Cell>();
                c.xPos = i;
                c.yPos = j;
                c.transform.localScale = cellScale;
                c.transform.localPosition = startPos +
                                            new Vector3(((float)i + 0.5f) * cellScale.x, 0,
                                                ((float)j + 0.5f) * cellScale.z);
                    cellList.Add(c);
            }
        }
    }

    public Cell GetClosestCellAtPos(Vector3 pos)
    {
        return cellList.OrderByDescending(x => Vector3.Distance(x.transform.position, pos)).Last();
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
