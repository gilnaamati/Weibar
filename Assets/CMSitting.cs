using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CMSitting : SMbase
{

    public float sittingDur = 1;


    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        throw new System.NotImplementedException();
    }

    protected override State OnUpdate()
    {
        if (curData.bodyState == SMdata.BodyState.Sitting)
        {
            return State.Success;
        }
        else if (curData.bodyState == SMdata.BodyState.Standing)
        {
            if (curData.posMarker.proximityList.Where(x => x.markerName == "Chair").ToList().Count > 0)
            {
                Debug.Log("I can sit");
            }
            else
            {
                var chair = curData.posMarker.GetClosestPosMarker("Chair");
                curData.posCM.tarPos = chair.transform.position;
                curData.posCM.UpdateState();
                return State.Running;
            }
        }

        return state;
    }

    
}
