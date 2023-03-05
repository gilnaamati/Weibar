using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public Transform marker;
    public LayerMask layerMask;
    public LayerMask itemMask;
    private Camera cam;
    public Item currentTouchedItem;
    public Cell currentTouchedCell;
    public CellGrid currentTouchedGrid;
    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
    }

    private void FixedUpdate()
    {
        CheckSurfaceColliders();
        CheckItemColliders();
    }

    void CheckItemColliders()
    {
        RaycastHit hit;
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, itemMask))
        {
            var item = hit.collider.GetComponentInParent<Item>();
            if (item != null)
            {
                if (currentTouchedItem != item)
                {
                    if (currentTouchedItem != null) currentTouchedItem.SetMarker(false);
                    currentTouchedItem = item;
                    item.marker.SetActive(true);
                }
            }
        }
        else
        {
            if (currentTouchedItem != null) currentTouchedItem.SetMarker(false);
            currentTouchedItem = null;
        }
    }
    
    void CheckSurfaceColliders()
    {
        RaycastHit hit;
        
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            marker.position = hit.point;
            var cg = hit.collider.GetComponentInParent<CellGrid>();
            if (cg != null)
            {
                if (currentTouchedGrid != cg) currentTouchedGrid = cg;
                var c = cg.GetClosestCellAtPos(hit.point);
                marker.position = c.transform.position;
                if (currentTouchedCell != c) currentTouchedCell = c;
            }
            else
            {
                currentTouchedGrid = null;
                currentTouchedCell = null;
            }
        }
        else
        {
            currentTouchedCell = null;
        }
    }
}
