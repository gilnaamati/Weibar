using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hand2D : MonoBehaviour
{
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
        customerH = FindObjectOfType<CustomerHandler>();
        itemH = FindObjectOfType<ItemHandler>();
        Cursor = this;
    }

    private void MouseData2D_MouseUpEvent()
    {
        mouseDown = false;
    }

    private void MouseData2D_MouseDownEvent()
    {
        mouseDown = true;
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
                        if (contentModule.curContentsAmount > 0)
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


