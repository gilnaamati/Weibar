using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMPosition : SMbase
{

    public Vector3 tarPos;
    public float proximityNeeded = 0.5f;
    public float speed = 1;

    protected override void OnStart()
    {
        
    }

    protected override void OnStop()
    {
        
    }

    

    protected override State OnUpdate()
    {
        var targetVector = Vector3.Normalize(tarPos - transform.position);
        transform.position += targetVector * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, tarPos) < proximityNeeded)
        {
            return State.Success;
        }

        else return State.Running;
       
    }

  
}
