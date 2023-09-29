using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHoldModule : MonoBehaviour
{
    public enum HandState
    {
      AboutToPick,
      Holding,
      AboutToDrink,
      Drinking,
      PickupUp,
      PuttingDown,
      Idle

    };


    public HandState handState;

    public CustHandModule hand;
    public PickupModule offeredItem;

    public List<PickupModule> ownedItemList = new List<PickupModule>();
    public PickupModule ownedItem;
    public PickupModule heldItem;

    float waitBeforeFirstSiptime = 1;
    float pickupDist = 0.1f;
    public List<Transform> ItemPositionList = new List<Transform>();
    public Transform Mouth;
    public Transform HoldPos;
    public float HandSpeed = 3;
    public PickupModule targetItem;

    private void Awake()
    {
        hand.HandReachedTargetEvent += CustomerHand_HandReachedTargetEvent;
        SetStateIdle(); 
        
    }

    void SetStateAboutToPick(PickupModule t)
    {
        targetItem = t;
        hand.SetTarget(t.transform);
        handState = HandState.AboutToPick;
    }
    
    void SetStateHolding()
    {
        handState = HandState.Holding;
    }

    void SetStateAboutToDrink()
    {
        handState = HandState.AboutToDrink;
    }

    void SetStateDrinking()
    {
        handState = HandState.Drinking;
    }

    void SetStateIdle()
    {
        handState = HandState.Idle;
        hand.curTar = HoldPos;      
    }

    void SetStatePickingUp(PickupModule t)
    {
        PickupItem(t);
        hand.SetTarget(HoldPos);
        handState = HandState.PickupUp;
    }

    private void CustomerHand_HandReachedTargetEvent()
    {
        if (handState == HandState.AboutToPick)
        {
            SetStatePickingUp(targetItem);
        }
        else if (handState == HandState.PickupUp)
        {
            SetStateHolding();
        }
    }

    public void SetOfferedItem(PickupModule d)
    {
        offeredItem = d;
        if (ItemAcceptable(d))
        {   
            ownedItemList.Add(offeredItem);
        }
    }

    public void DecideToPickUpItem()
    {
        if (ownedItemList.Count > 0)
        {
            SetStateAboutToPick(ownedItemList.GetRandom());
        }
    }

    public void PickupItem(PickupModule i)
    {
        i.SetStateHeldByCustomer(this);
        heldItem = i;
    }

    bool ItemAcceptable(PickupModule d)
    {
        return true;
    }

    
     
}
