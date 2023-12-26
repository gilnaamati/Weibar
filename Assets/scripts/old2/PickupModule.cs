using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupModule : MonoBehaviour
{
    public event Action SetStateHeldByPlayerEvent = () => { };
    public event Action SetStateIdleEvent = () => { };
    public event Action<CustomerHoldModule> SetStateHeldByCustomerEvent = (c) => { };

    public enum PickupState
    {
        Idle,
        HeldByPlayer,
        HeldByCustomer
    }

    public PickupState pickupState;
    public Transform handle;
    public float dragLerp = 20;

    private bool playerHoldingItem = false;
    private ItemBase itemBase;
    Rigidbody2D rb;

    private CustomerHoldModule curCustomer; //the customer currently holding me.

    public ContentModule cm;
    public PourTargetModule ptm;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        itemBase = GetComponent<ItemBase>();
        SetStateIdle();
        cm = GetComponent<ContentModule>();
        ptm = GetComponent<PourTargetModule>();
    }

    private void OnItemPickedUpEvent(PickupModule obj)
    {
        if (obj != this) playerHoldingItem = true;
    }

    private void FixedUpdate()
    {
        if (pickupState == PickupState.HeldByPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, MouseData2D.Inst.mouseWorldPos - handle.localPosition,
                Time.fixedDeltaTime * dragLerp);
        }
        else if (pickupState == PickupState.HeldByCustomer)
        {
            transform.position = Vector3.Lerp(transform.position, curCustomer.hand.transform.position - handle.localPosition,
               Time.fixedDeltaTime * dragLerp);
        }
    }
    
    public void SetStateHeldByPlayer() // the colliders are off on this one, so it can't be touched. 
    {
        pickupState = PickupState.HeldByPlayer;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponentInChildren<TouchCollidersHandlers>().ToggleColliders(false);
        SetStateHeldByPlayerEvent();
    }

    public void SetStateHeldByCustomer(CustomerHoldModule c)
    {
        curCustomer = c;
        pickupState = PickupState.HeldByCustomer;
        rb.bodyType = RigidbodyType2D.Kinematic;
        GetComponentInChildren<TouchCollidersHandlers>().ToggleColliders(false);
        SetStateHeldByCustomerEvent(c);
    }

    public void SetStateIdle()
    {
        pickupState = PickupState.Idle;
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponentInChildren<TouchCollidersHandlers>().ToggleColliders(true);
        SetStateIdleEvent();
    }
}
