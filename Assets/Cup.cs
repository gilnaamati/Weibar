using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MovableItem, IPourIntoable
{
   
    public override void SetHeld()
    {
        Debug.Log(physicalCollider.gameObject);
        base.SetHeld();
        
    }
}
