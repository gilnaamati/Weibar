using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public static event Action<PickupModule, ItemBase> ItemInteractionEvent = (xxx, yyy) => { };

    public static ItemHandler Inst;

    public ItemBase topHoverItem;

    public List<Item2D> itemList = new List<Item2D>();

    int curMaskLayer = 0;

    public List<spriteVisualSorter> itemVisualsList = new List<spriteVisualSorter>();

    int curMaxSortInd = 0;

    public List<ItemBase> itemHoverList = new List<ItemBase>();

    public PickupModule currentlyHeldModule;

    private void Awake()
    {
        Inst = this;
        SetItemSortVisuals();
        SetItemMaskIndexes();
        ItemBase.HoverEnterEvent += ItemBase_HoverEnterEvent;
        ItemBase.HoverExitEvent += ItemBase_HoverExitEvent;
        MouseData2D.MouseDownEvent += MouseData2DOnMouseDownEvent;
        MouseData2D.MouseUpEvent += MouseData2DOnMouseUpEvent;

    }

    private void MouseData2DOnMouseUpEvent()
    {
        if (currentlyHeldModule != null)
        {
            currentlyHeldModule.SetStateIdle();
            currentlyHeldModule = null;
        }
    }

    private void MouseData2DOnMouseDownEvent()
    {
        if (itemHoverList.Count > 0)
        {
            var item = itemHoverList.First(x => x.itemTouchState == ItemBase.ItemTouchState.TopHover);
            var module = item.GetComponent<PickupModule>();
            if (module != null)
            {
                module.SetStateHeld();
                currentlyHeldModule = module;
            }
        }
    }

    private void ItemBase_HoverExitEvent(ItemBase obj)
    {
        if (itemHoverList.Contains(obj))
        {
            itemHoverList.Remove(obj);
            obj.SetStateNoHover();
            UpdateHoverList();
        }
    }
    private void ItemBase_HoverEnterEvent(ItemBase obj)
    {
        if (!itemHoverList.Contains(obj))
        {
            itemHoverList.Add(obj);
            obj.SetStateHover();
            UpdateHoverList();
        }
    }

    private void FixedUpdate()
    {

    }

    void UpdateHoverList()
    {
        if (itemHoverList.Count == 0)
        {
            topHoverItem = null;
            ItemInteractionEvent(currentlyHeldModule, null);
            return;
        }
        foreach (var v in itemHoverList) v.SetStateHover();
        topHoverItem = itemHoverList.OrderByDescending(x => x.GetComponentInChildren<spriteVisualSorter>().sortInd).First();
        topHoverItem.SetStateTopHover();
        ItemInteractionEvent(currentlyHeldModule, topHoverItem);
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

    void SetItemMaskIndexes()
    {
        var maskrList = GetComponentsInChildren<MaskHandler>().ToList();
        foreach (var v in maskrList)
        {
            v.SetMask(curMaskLayer);
            curMaskLayer++;
        }

    }
}
