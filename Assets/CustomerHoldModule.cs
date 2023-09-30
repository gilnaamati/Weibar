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
    public Transform MouthPos;
    public Transform HoldPos;
    public float HandSpeed = 3;
    public PickupModule targetItem;

    string dominantUrge;


    public TextMeshPro urgeIndicator;
    private void Awake()
    {
        hand.HandReachedTargetEvent += CustomerHand_HandReachedTargetEvent;
        SetStateIdle();
        SetDominantUrge("chill");
        
    }

    public void SetDominantUrge(string s)
    {
        dominantUrge = s;
        urgeIndicator.text = s;
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
        hand.curTar = MouthPos;
    }

    void SetStateAboutToDrink(PickupModule t)
    {
        Debug.Log("I'm in the state wtf");
        PickupItem(t);
        handState = HandState.AboutToDrink;
        hand.SetTarget(MouthPos);
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
            if (dominantUrge == "drink")
            {
                Debug.Log("about to set drink state");
                SetStateAboutToDrink(targetItem);
            }
            else
            {
                SetStatePickingUp(targetItem);
            }
           
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

    private void Update()
    {
        switch (handState)
        {
            case HandState.Idle:
                if (dominantUrge == "drink")
                {
                    var d = ownedItemList.Where(x => x.GetComponent<KeywordModule>().keywordList.Contains("drink")).ToList();
                    if (d.Count > 0)
                    {
                        SetStateAboutToPick(d.GetRandom());
                    }
                }
                break;
            case HandState.Drinking:
                break;
        }
    }



}
