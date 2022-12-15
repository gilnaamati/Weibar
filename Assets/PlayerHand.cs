using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public Transform marker;
    public LayerMask layerMask;
    private Camera cam;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            marker.position = hit.point;
        }


    }
}
