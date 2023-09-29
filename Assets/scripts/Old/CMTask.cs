using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMTask : SMbase
{
    public SMdata.BodyState bodyState;
    public List<string> proximityNameList = new List<string>();

    protected override void OnStart()
    {
       
    }

    protected override void OnStop()
    {
       
    }

    protected override State OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}
