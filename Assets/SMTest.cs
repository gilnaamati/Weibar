using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMTest : SMbase
{
    public SMdata curData;

    public float var1goal = 2;
    public float var1changeRate = 0.1f;

    protected override void OnStart()
    {
        Debug.Log("onStart");
        curData = GetComponent<SMdata>();
        curData.var1 = 0;
    }

    protected override void OnStop()
    {
        Debug.Log("onStop");
    }

    protected override State OnUpdate()
    {
        if (curData.var1 < var1goal)
        {
            curData.var1 += var1changeRate * Time.deltaTime;
            state = State.Running;
        }
        else
        {
            curData.var1 = var1goal;
            state = State.Success;
        }

        return state;
    }
}
