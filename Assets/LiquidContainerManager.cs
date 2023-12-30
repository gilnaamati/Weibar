using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidContainerManager : MonoBehaviour
{
    public Transform normalPourPosition;
    public Transform endDownPourPosition;
    public Transform startPourPosition;
    public Transform visualContainer;
    public Transform normalLiquidFullPos;
    public Transform normalLiquidEmptyPos;
    public Transform endLiquidPourPos;
    public Transform startLiquidPourPos;
    public Transform liquidVisualContainer;
    public float pourLerp = 10;
    private float lastPourAngleZ = 0;
    public float PourAngleThreshold = 2;

    private void Awake()
    {
        lastPourAngleZ = Quaternion.Lerp(startPourPosition.localRotation, endDownPourPosition.localRotation, 0).eulerAngles.z;
        
    }

    public void SetStartPositions(float curPourRatio)
    {
        liquidVisualContainer.localPosition = Vector3.Lerp(normalLiquidFullPos.localPosition, normalLiquidEmptyPos.localPosition, 1-curPourRatio);
        visualContainer.localPosition = normalPourPosition.localPosition;
        visualContainer.localRotation =normalPourPosition.localRotation; 
        
    }
    
    public void UpdatePourState(float curPourRatio)
    {
        var curBottlePourPos = Vector3.Lerp(startPourPosition.localPosition, endDownPourPosition.localPosition, 1-curPourRatio);
        var curBottlePourRot = Quaternion.Lerp(startPourPosition.localRotation, endDownPourPosition.localRotation, 1-curPourRatio);
        visualContainer.localPosition = Vector3.Lerp(visualContainer.localPosition, curBottlePourPos, pourLerp * Time.deltaTime);
        visualContainer.localRotation = Quaternion.Lerp(visualContainer.localRotation, curBottlePourRot, pourLerp * Time.deltaTime);
        lastPourAngleZ = curBottlePourRot.eulerAngles.z;
        
        var curLiquidPourPos = Vector3.Lerp(startLiquidPourPos.localPosition, endLiquidPourPos.localPosition, 1-curPourRatio);
        liquidVisualContainer.localPosition = Vector3.Lerp(liquidVisualContainer.localPosition, curLiquidPourPos, pourLerp * Time.deltaTime);
       
    }

    public void UpdateNormalState(float curPourRatio)
    {
        var curLiquidNormalPos = Vector3.Lerp(normalLiquidFullPos.localPosition, normalLiquidEmptyPos.localPosition, 1-curPourRatio);
        liquidVisualContainer.localPosition = Vector3.Lerp(liquidVisualContainer.localPosition, curLiquidNormalPos,
            pourLerp * Time.deltaTime);
        visualContainer.localPosition = Vector3.Lerp(visualContainer.localPosition, normalPourPosition.localPosition, pourLerp * Time.deltaTime);
        visualContainer.localRotation = Quaternion.Lerp(visualContainer.localRotation, normalPourPosition.localRotation, pourLerp * Time.deltaTime);
    }

    
    public bool ComparePourAngle(float curPourRatio)
    {
        lastPourAngleZ = Quaternion.Lerp(startPourPosition.localRotation, endDownPourPosition.localRotation, 1-curPourRatio).eulerAngles.z;
        if (Mathf.DeltaAngle(visualContainer.localRotation.eulerAngles.z,lastPourAngleZ ) < PourAngleThreshold)
        {
            return true;
        }
        return false;
    }
}
