using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CustomerHoldModule : MonoBehaviour
{
    public enum HandState
    {
      AboutToPick,
      Holding,
      AboutToDrink,
      Drinking,
      PickingUp,
      PuttingDown,
      Idle
    };

    public event Action<HandState> ChangeStateEvent = (x) => { };
    public event Action<PickupModule> ItemPickedEvent = (x) => { };
    public event Action<DrinkModule> StartDrinkingEvent = (x) => { };
    public HandState handState;

    public CustHandModule hand;
    public PickupModule offeredItem;

    public List<PickupModule> ownedItemList = new List<PickupModule>();
    public PickupModule ownedItem;
    public PickupModule heldItem;

    float waitBeforeFirstSiptime = 1;
    float pickupDist = 0.1f;
    public List<Transform> itemPosList = new List<Transform>();
    public Transform mouthPos;
    public Transform holdPos;
    public Transform idlePos;
    public float handSpeed = 15;
    public PickupModule pickupTargetItem;

    string dominantUrge;

    public TextMeshPro urgeIndicator;

    CustomerMouthModule mouthModule;

    private void Awake()
    {
        hand.HandReachedTargetEvent += CustomerHand_HandReachedTargetEvent;
        mouthModule = GetComponent<CustomerMouthModule>();
        mouthModule.ChangeMouthStateEvent += MouthModule_ChangeMouthStateEvent;
        SetStateIdle();
        SetDominantUrge("chill");
        
    }

    private void MouthModule_ChangeMouthStateEvent(CustomerMouthModule.MouthState obj)
    {
        if (obj == CustomerMouthModule.MouthState.Swallowing)
        {
            if (dominantUrge == "drink" && heldItem.cm.curConAm > 0)
            {
                SetStateHolding();
            }
            else
            {
                SetStatePuttingDown();
            }
           
        }
        else if (obj == CustomerMouthModule.MouthState.Idle)
        {
            if (handState == HandState.Holding)
            {
                if (dominantUrge == "drink")
                {
                    if (heldItem.cm.curConAm > 0)
                    {
                        SetStateAboutToDrink(heldItem);
                    }
                    else
                    {
                        SetStatePuttingDown();
                    }
                }
            }
        } 
    }

    public void SetDominantUrge(string s)
    {
        dominantUrge = s;
        urgeIndicator.text = s;
    }

    void SetStatePuttingDown()
    {
        SetState(HandState.PuttingDown);
        hand.SetTarget(itemPosList.GetRandom());
    }

    void SetStateAboutToPick(PickupModule t)
    {
        pickupTargetItem = t;
        hand.SetTarget(t.transform);
        SetState(HandState.AboutToPick);
    }
    
    void SetStateHolding()
    {
        SetState(HandState.Holding);
        hand.SetTarget(holdPos);
    }

    void SetStateAboutToDrink(PickupModule t)
    {
        PickupItem(t);
        SetState(HandState.AboutToDrink);
        hand.SetTarget(mouthPos);
    }

    void SetStateDrinking()
    {
        SetState(HandState.Drinking);
        StartDrinkingEvent(heldItem.GetComponent<DrinkModule>());
    }

    void SetStateIdle()
    {
        if (heldItem != null)
        {
            heldItem.SetStateIdle();
            heldItem = null;
        }
        SetState(HandState.Idle);
        hand.SetTarget(idlePos);
    }

    void SetStatePickingUp(PickupModule t)
    {
        PickupItem(t);
        hand.SetTarget(holdPos);
        SetState(HandState.PickingUp);
    }

    private void CustomerHand_HandReachedTargetEvent()
    {
        switch (handState)
        {
            case HandState.AboutToDrink:
                 SetStateDrinking();   
                 break;
            case HandState.AboutToPick:
                if (dominantUrge == "drink")
                {
                    Debug.Log("about to set drink state");
                    SetStateAboutToDrink(pickupTargetItem);
                }
                else
                {
                    SetStatePickingUp(pickupTargetItem);
                }
                break;
            case HandState.PuttingDown:
                SetStateIdle();
                break;


            case HandState.PickingUp:

                SetStateHolding();
                break;
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

    private void Update()
    {
        switch (handState)
        {
            case HandState.Idle:
                if (dominantUrge == "drink")
                {
                    var d = ownedItemList.Where(x => x.GetComponent<KeywordModule>().keywordList.Contains("drink") && x.cm.curConAm > 0 && x.ptm.pourTargetState != PourTargetModule.PourTargetState.PouredInto).ToList();
                    if (d.Count > 0)
                    {
                        SetStateAboutToPick(d.GetRandom());
                    }
                }
                break;
            case HandState.Drinking:
                break;
            case HandState.AboutToPick:
                if (pickupTargetItem.ptm.pourTargetState != PourTargetModule.PourTargetState.Idle)
                {
                    SetStateIdle();
                }

                break;

        }
    }

    void SetState(HandState s)
    {
        Debug.Log("handstate: " + s.ToString());
        handState = s;
        ChangeStateEvent(s);
    }
}
