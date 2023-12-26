using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MovableItem, IPourable
{
    public float currentContents;
    public float maxContents;
    public float totalPourDuration = 4;
    public Transform normalPourPosition;
    public Transform endDownPourPosition;
    public Transform startPourPosition;
    public Transform visualContainer;
    public float pourLerp = 20;
    [SerializeField]
    IPourIntoable currentPourIntoable;
    [SerializeField]
    bool _pouring;
    public override void NewIinteractableTouching(IInteractable interactable)
    {
        if (_interactableTouchingList.Contains(interactable)) return;
        base.NewIinteractableTouching(interactable);
        if (!_held) return;
        if (_pouring) return;
        if (interactable is IPourIntoable)
        {
            StartPouring(interactable as IPourIntoable);
        }
    }
    
    public override void NewInteractableNotTouching(IInteractable interactable)
    {
        base.NewInteractableNotTouching(interactable);
        if (_interactableTouchingList.Contains(interactable)) return;
        
            if (currentPourIntoable == interactable as IPourIntoable)
            {
                StopPouring();
            }
    }
    public void StartPouring(IPourIntoable pourIntoable)
    {
        currentPourIntoable = pourIntoable;
        _pouring = true;
    }

    private void Update()
    {
        if (_pouring)
        {
            Pour();
        }
        else
        {
            visualContainer.localPosition = Vector3.Lerp(visualContainer.localPosition, normalPourPosition.localPosition, pourLerp * Time.deltaTime);
            visualContainer.localRotation = Quaternion.Lerp(visualContainer.localRotation, normalPourPosition.localRotation, pourLerp * Time.deltaTime);
        }
    }
    
    void Pour()
    {
        var pourSpeed = maxContents/ totalPourDuration;
        var curPourRatio = currentContents / maxContents;
        var curPourPos = Vector3.Lerp(startPourPosition.localPosition, endDownPourPosition.localPosition, 1-curPourRatio);
        var curPourRot = Quaternion.Lerp(startPourPosition.localRotation, endDownPourPosition.localRotation, 1-curPourRatio);
        visualContainer.localPosition = Vector3.Lerp(visualContainer.localPosition, curPourPos, pourLerp * Time.deltaTime);
        visualContainer.localRotation = Quaternion.Lerp(visualContainer.localRotation, curPourRot, pourLerp * Time.deltaTime);
        currentContents -= Time.deltaTime * pourSpeed;
        if (currentContents <= 0)
        {
            currentContents = 0;
            StopPouring();
        }
    }
    
    public void StopPouring()
    {
        currentPourIntoable = null;
        _pouring = false;
    }
}
