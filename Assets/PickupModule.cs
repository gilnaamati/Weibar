using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupModule : MonoBehaviour
{
    
    public enum PickupState
    {
        Idle,
        Held
    }

    public PickupState pickupState;
    public Transform handle;
    public float dragLerp = 20;

    private bool playerHoldingItem = false;
    private ItemBase itemBase;
    Rigidbody2D rb;
    private void Awake()
    {
      
        rb = GetComponent<Rigidbody2D>();
        itemBase = GetComponent<ItemBase>();
        SetStateIdle();
    }

    private void OnItemPickedUpEvent(PickupModule obj)
    {
        if (obj != this) playerHoldingItem = true;
    }

    private void FixedUpdate()
    {
        if (pickupState == PickupState.Held)
        {
            transform.position = Vector3.Lerp(transform.position, MouseData2D.Inst.mouseWorldPos,
                Time.fixedDeltaTime * dragLerp);
        }
    }
    
    public void SetStateHeld()
    {
        pickupState = PickupState.Held;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponentInChildren<TouchCollidersHandlers>().ToggleColliders(false);
    }

    public void SetStateIdle()
    {
        pickupState = PickupState.Idle;
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponentInChildren<TouchCollidersHandlers>().ToggleColliders(true);
    }
    
  
}
