using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

[Serializable]
public class ConditionSet
{
    public List<ConditionSet> conditionSetList = new List<ConditionSet>();
    public SMdata.BodyState bodyState;
    public List<string> proximityList = new List<string>();
}

public class SMbrain : SMbase
{


    public List<ConditionSet> conditionList = new List<ConditionSet>();
    


    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    private void Update()
    {
        OnUpdate();
    }

    protected override State OnUpdate()
    {
        if (conditionList.Count == 0)
            return State.Success;

        var curCondition = conditionList.Last();

        if (curCondition.bodyState == curData.bodyState)
        {

        }

        return state;
      
    }
}
