using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class MouseData2D : MonoBehaviour
{
    public static event Action MouseDownEvent = () => { };
    public static event Action MouseUpEvent = () => { };
    
    public Camera cam;
    public static MouseData2D Inst;
    public Vector3 mouseWorldPos;
    public Vector2 mouseVPpos;
    public float dist = 10;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
        cam = GetComponent<Camera>();
    }
    
    // Update is called once per frame
  
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(Time.time.ToString());
            MouseDownEvent();
        }

        if (Input.GetMouseButtonUp(0))
        {
            MouseUpEvent();
        }
        
        var mousePos = Input.mousePosition;
        mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, dist));
        mouseVPpos = cam.ScreenToViewportPoint(new Vector3(mousePos.x, mousePos.y, dist));
    }
}
