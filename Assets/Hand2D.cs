using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand2D : MonoBehaviour
{
   
    void Update()
    {
        transform.position = MouseData2D.Inst.mouseWorldPos;
    }
}
