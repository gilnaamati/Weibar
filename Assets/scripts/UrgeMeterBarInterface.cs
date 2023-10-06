using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(UrgeMeterBar))]
[ExecuteInEditMode]
public class UrgeMeterBarInterface : MonoBehaviour
{
    public bool updateBar = false;
    void Update()
    {
        if (updateBar)
        {
            GetComponent<UrgeMeterBar>().InitMeter();
        }  
    }
}
