using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class FluidSimActivator : MonoBehaviour
{
    public bool gen = false;
    
    public FluidSim fluidSim;

    private void Update()
    {
        if (!Application.isPlaying)
        {
            if (fluidSim != null)
            {
                if (gen == true)
                {
                   
                    fluidSim.Gen();
                }
            }
        }
    }
}
