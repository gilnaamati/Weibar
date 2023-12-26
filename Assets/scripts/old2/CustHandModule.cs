using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustHandModule : MonoBehaviour
{
    public event Action HandReachedTargetEvent = () => { };

    public float speed = 5;
    public Transform curTar;
    public Vector3 curRealPos;
    public float lerp = 10;
    public float stopDist = 0.25f;
    bool arrivedAtTarget;

    private void Awake()
    {
        arrivedAtTarget = false;
        curRealPos = transform.position;
    }


    public void SetTarget(Transform t)
    {
        if (curTar != t)
        {
            arrivedAtTarget = false;
            curTar = t;
        }
    }   

    private void FixedUpdate()
    {
        if (!arrivedAtTarget)
        {
            if (Vector3.Distance(transform.position, curTar.position) <= stopDist)
            {
                arrivedAtTarget = true;
              //  Debug.Log("Hand arrived at target");
                HandReachedTargetEvent();
                return;
            }

            var v = curTar.position - curRealPos;   
            if (v.magnitude > stopDist)
            {
                curRealPos += v.normalized * speed * Time.fixedDeltaTime;
            }

            //transform.position = curRealPos;    
            transform.position = Vector3.Lerp(transform.position, curRealPos, lerp * Time.fixedDeltaTime);
        }
        
    }

}
