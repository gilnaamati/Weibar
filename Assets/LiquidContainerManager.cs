using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidContainerManager : MonoBehaviour
{
    public Transform normalPourPosition;
    public Transform endDownPourPosition;
    public Transform startPourPosition;
    public Transform visualContainer;
    public float pourLerp = 10;
    private float lastPourAngleZ = 0;
    public float PourAngleThreshold = 2;
    public bool UpdatePourState(float curPourRatio)
    {
        var curPourPos = Vector3.Lerp(startPourPosition.localPosition, endDownPourPosition.localPosition, 1-curPourRatio);
        var curPourRot = Quaternion.Lerp(startPourPosition.localRotation, endDownPourPosition.localRotation, 1-curPourRatio);
        visualContainer.localPosition = Vector3.Lerp(visualContainer.localPosition, curPourPos, pourLerp * Time.deltaTime);
        visualContainer.localRotation = Quaternion.Lerp(visualContainer.localRotation, curPourRot, pourLerp * Time.deltaTime);
        lastPourAngleZ = curPourRot.eulerAngles.z;
        return ComparePourAngle();
    }

    public bool UpdateNormalState()
    {
        visualContainer.localPosition = Vector3.Lerp(visualContainer.localPosition, normalPourPosition.localPosition, pourLerp * Time.deltaTime);
        visualContainer.localRotation = Quaternion.Lerp(visualContainer.localRotation, normalPourPosition.localRotation, pourLerp * Time.deltaTime);
        return ComparePourAngle();
    }

    bool ComparePourAngle()
    {
        if (Mathf.DeltaAngle(visualContainer.localRotation.eulerAngles.z,lastPourAngleZ ) < PourAngleThreshold)
        {
            return true;
        }
        return false;
    }
}
