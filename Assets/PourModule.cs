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

    public Transform visuals;

    public Transform pourHandle;
    
    PickupModule pm;

    public float curAngle;
    public float pourRotateSpeed;
    public float heldRotateSpeed;
    public float pourRate = 10;
    public float angleWhenEmpty;
    public float angleWhenFull;
    public float anglePourMargin = 1;
    public float visualsLerp = 20;
    public float pourPosLerp = 10;
    public float dropOffset = 2;
    public float dropPushForce = 10;
    float curVisualsAngle;
    float curPourAngle;
    float curPourDeltaAngle;
    float curHeldDeltaAngle;

    Vector3 orgHandlePos;
    private ContentModule cm;
    private void Awake()
    {
        cm = GetComponent<ContentModule>();
        pm = GetComponent<PickupModule>();
        pm.SetStateHeldEvent += PourModule_SetStateHeldEvent;
        pm.SetStateIdleEvent += PourModule_SetStateIdleEvent;
        orgHandlePos = pm.handle.localPosition;
        ItemHandler.ItemInteractionEvent += ItemHandler_ItemInteractionEvent;
    }

    private void ItemHandler_ItemInteractionEvent(PickupModule arg1, ItemBase arg2)
    {
        if (arg1 != GetComponent<PickupModule>()) return;

        if (arg2 == null)
        {
            if (pourState == PourState.Pouring) SetStateHeld();
            return;
        }

        if (arg2.GetComponent<PourTargetModule>() == null)
        {
            if (pourState == PourState.Pouring) SetStateHeld();
        }
        else
        {
            SetStatePouring(arg2.GetComponent<PourTargetModule>());
        }
    }

   

    private void PourModule_SetStateIdleEvent()
    {
        if (pourState != PourState.Pouring) curPourTarget = null;

         SetStateIdle();
    }

    private void PourModule_SetStateHeldEvent()
    {
        SetStateHeld();
    }

    public PourState pourState = PourState.Idle;

  

    private void FixedUpdate()
    { 
        if (pourState == PourState.Pouring) UpdatePour();
        else if (pourState == PourState.Held) UpdateHeld();
        else if (pourState == PourState.Idle) UpdateIdle();
        pm.handle.localPosition = Vector3.Lerp(pm.handle.localPosition, pourHandlePosTar, pourPosLerp * Time.fixedDeltaTime);
       
        UpdateAngle();
        UpdateVisuals();
    }

    void UpdateIdle()
    {
        UpdateHeld();
        if (curPourTarget != null)
        {
            if (Mathf.Abs(curPourTarget.transform.position.x - transform.position.x) < dropOffset)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector3.right * dropPushForce * curPourAngle * Time.fixedDeltaTime, ForceMode2D.Force);
            }
            else
            {
                curPourTarget = null;
            }
        }
      
    }
    void UpdatePour()
    {
        
        if (Mathf.Abs(curPourDeltaAngle) >= anglePourMargin) curAngle += pourRotateSpeed * Mathf.Sign(curPourDeltaAngle) * Time.fixedDeltaTime;
        else curAngle = curPourAngle; 
    }

    void UpdateHeld()
    {

        if (Mathf.Abs(curHeldDeltaAngle) >= anglePourMargin) curAngle += heldRotateSpeed * Mathf.Sign(curHeldDeltaAngle) * Time.fixedDeltaTime;
        else curAngle = 0;
    }

    void UpdateAngle()
    {
        curPourAngle = Mathf.Lerp(angleWhenEmpty, angleWhenFull, cm.curContents / cm.maxContents)*pourDir;
        curHeldDeltaAngle = Mathf.DeltaAngle(curAngle, 0);
        curPourDeltaAngle = Mathf.DeltaAngle(curAngle, curPourAngle);
        if (Mathf.Abs(curPourDeltaAngle) < anglePourMargin) TransferLiquids();
    }

    void UpdateVisuals()
    {

        curVisualsAngle = Mathf.LerpAngle(curVisualsAngle, curAngle, visualsLerp * Time.fixedDeltaTime);
        visuals.eulerAngles = new Vector3(0, 0, curVisualsAngle);
        pourDummy.eulerAngles = new Vector3(0, 0, curAngle);
        if (curAngle != 0) cm.UpdateVisuals();
    }

    void TransferLiquids()
    {
        var transferAmount = Mathf.Min(cm.curContents, pourRate * Time.fixedDeltaTime);
        cm.ChangeContentAmount(-transferAmount);
        if (curPourTarget != null)
        {
            curPourTarget.RecieveLiquids(transferAmount);
        }
    }

    float pourDir = 1;
    PourTargetModule curPourTarget;
    Vector3 pourHandlePosTar;
    public void SetStatePouring(PourTargetModule target)
    {
        curPourTarget = target;
        float newPourDir = Mathf.Sign(transform.position.x - curPourTarget.transform.position.x);
        if (newPourDir != pourDir)
        {
            if (Math.Abs(curAngle) < 30) pourDir = newPourDir;
        }
        pourHandlePosTar = new Vector3(pourHandle.localPosition.x * pourDir, pourHandle.localPosition.y, pm.handle.localPosition.z);
        pourState = PourState.Pouring;
    }

    public void SetStateHeld()
    {
        curPourTarget = null;
        pourHandlePosTar = orgHandlePos;
        pourState = PourState.Held;
    }

    public void SetStateIdle()
    {

        pourHandlePosTar = orgHandlePos;
        pourState = PourState.Idle;
    }
}
