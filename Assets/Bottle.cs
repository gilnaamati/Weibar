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
    private LiquidContainerManager lcm;
    public float pourLerp = 20;
    [SerializeField]
    IPourIntoable currentPourIntoable;
    [SerializeField]
    bool _pouring;
   
    protected override void Awake()
    {
        base.Awake();
        lcm = GetComponentInChildren<LiquidContainerManager>();
        lcm.SetStartPositions(currentContents / maxContents);
    }

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
            lcm.UpdateNormalState(currentContents / maxContents);
            if (lcm.ComparePourAngle(currentContents / maxContents))
            {
                var pourSpeed = maxContents/ totalPourDuration;
                currentContents -= Time.deltaTime * pourSpeed;
            }
        }
    }
    
    void Pour()
    {
        lcm.UpdatePourState(currentContents / maxContents);
        if (lcm.ComparePourAngle(currentContents / maxContents))
        {
            var pourSpeed = maxContents/ totalPourDuration;
            currentContents -= Time.deltaTime * pourSpeed;
        }
        
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
