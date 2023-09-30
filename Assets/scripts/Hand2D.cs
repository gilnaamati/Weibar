using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hand2D : MonoBehaviour
{
    public static event Action<string> HandDownEvent = (x) => { };
    public static event Action<string> HandUpEvent = (x) => { };
    public static event Action<PickupModule> ItemOfferedEvent = (x) => { };
    public static Hand2D Cursor;
    public string curKey;
    public PickupModule curItem;
    public TextMeshPro curKeyText;

    CustomerHandler customerH;
    ItemHandler itemH;

    bool mouseDown;

    private void Awake()
    {
        MouseData2D.MouseDownEvent += MouseData2D_MouseDownEvent;
        MouseData2D.MouseUpEvent += MouseData2D_MouseUpEvent; 
        customerH = FindObjectOfType<CustomerHandler>(); // customer handler
        itemH = FindObjectOfType<ItemHandler>(); // item handler
        Cursor = this;
    }

    private void MouseData2D_MouseUpEvent()
    {
        if (curKey.Contains( "Offer"))
        {
            ItemOfferedEvent(curItem);
        }
        mouseDown = false;
        HandUpEvent(curKey);
    }

    private void MouseData2D_MouseDownEvent()
    {
        mouseDown = true;
        HandDownEvent(curKey);
    }

    void Update()
    {
        curItem = itemH.currentlyHeldModule;
        transform.position = MouseData2D.Inst.mouseWorldPos;
        SetCurKey();
    }

    void SetCurKey()
    {
        if (!mouseDown)
        {
            if (itemH.topHoverItem != null)
            {
                var pickupModule = itemH.topHoverItem.GetComponent<PickupModule>();
                if (pickupModule != null)
                {
                    SetKey("Pick Up");
                    return;
                }
            }

            if (customerH.topHoverCustomer != null)
            {
                SetKey("Interact");
                return;
            }
        }
        else
        {
            if (itemH.currentlyHeldModule != null)
            {
                if (itemH.topHoverItem != null)
                {
                    var pourModule = itemH.currentlyHeldModule.GetComponent<PourModule>();
                    if (pourModule != null)
                    {
                       if (pourModule.pourState == PourModule.PourState.Pouring)
                        {
                            SetKey("Pouring");
                            return;
                        }
                       
                    }

                    SetKey("Drop");
                    return;
                }

                if (customerH.topHoverCustomer != null)
                {
                    var contentModule = itemH.currentlyHeldModule.GetComponent<ContentModule>();

                    if (contentModule != null)
                    {
                        if (contentModule.curConAm > 0)
                        {
                            SetKey("Offer");
                            return;
                        }
                    }             
                }
                SetKey("Drop");
                return;
            }
        }


        SetKey("Idle");
    }

    void SetKey(string k)
    {
        curKey = k;
        curKeyText.text = k;
    }
}


