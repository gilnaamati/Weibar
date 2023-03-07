using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourModule : MonoBehaviour
{
    public enum PourState
    {
        Idle,
        Held,
        Pouring
    }

    public Transform pourDummy;

    private ContentModule cm;
    private void Awake()
    {
        cm = GetComponent<ContentModule>();
    }


    public PourState pourState = PourState.Idle;

    public float curAngle;
    public float rotateSpeed;
    public float pourRate = 10;
    public float angleWhenEmpty;
    public float angleWhenFull;
    public float anglePourMargin = 1;

    private void FixedUpdate()
    {
        if (pourState == PourState.Pouring)
        {
            UpdatePour();
        }
    }

    void UpdatePour()
    {
        float curPourAngle = Mathf.Lerp(angleWhenEmpty, angleWhenFull, cm.curContents / cm.maxContents);
        var deltaAngle = Mathf.DeltaAngle(curAngle, curPourAngle);
        if (deltaAngle < anglePourMargin)
        {
            TransferLiquids();
        }
        else
        {
            curAngle += rotateSpeed * Mathf.Sign(deltaAngle)*Time.fixedDeltaTime;
        }

        pourDummy.eulerAngles = new Vector3(0, 0, curAngle);
    }

    void TransferLiquids()
    {
        cm.ChangeContentAmount(-pourRate*Time.fixedDeltaTime);
    }
    
    public void SetStatePouring()
    {
        pourState = PourState.Pouring;
    }
}
