using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CustomerHandler : MonoBehaviour
{
    //public static event Action<PickupModule, CustomerBase> ItemInteractionEvent = (xxx, yyy) => { };

    public List<CustomerBase> hoverList= new List<CustomerBase>();
    public static CustomerHandler Inst;
    public CustomerBase topHoverCustomer;
    int curMaxSortInd = 0;
    private void Awake()
    {
        Inst = this;
        SetItemSortVisuals();
        CustomerBase.HoverEnterEvent += Customer_HoverEnterEvent;
        CustomerBase.HoverExitEvent += Customer_HoverExitEvent;
        Hand2D.ItemOfferedEvent += Hand2D_ItemOfferedEvent;
    }

    private void Hand2D_ItemOfferedEvent(PickupModule obj)
    {
        var c = topHoverCustomer.GetComponent<CustomerDrinkModule>();
        c.SetOwnedDrink(obj);
    }

    private void Customer_HoverExitEvent(CustomerBase obj)
    {
        if (hoverList.Contains(obj))
        {
            hoverList.Remove(obj);
            obj.SetStateNoHover();
            UpdateHoverList();
        }
    }

    private void Customer_HoverEnterEvent(CustomerBase obj)
    {
        if (!hoverList.Contains(obj))
        {
            hoverList.Add(obj);
            obj.SetStateHover();
            UpdateHoverList();
        }
    }

    void UpdateHoverList()
    {
        if (hoverList.Count == 0)
        {
            topHoverCustomer = null;
            return;
        }
        foreach (var v in hoverList) v.SetStateHover();
        topHoverCustomer = hoverList.OrderByDescending(x => x.GetComponentInChildren<spriteVisualSorter>().sortInd).First();
        topHoverCustomer.SetStateTopHover();

    }

    void SetItemSortVisuals()
    {
        var l = GetComponentsInChildren<spriteVisualSorter>().ToList();

        foreach (var v in l)
        {
            v.SetSortingLayerIndex(curMaxSortInd);
            curMaxSortInd++;
        }
    }


}
