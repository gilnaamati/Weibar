using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public List<Item2D> itemList = new List<Item2D>();

    int curMaskLayer = 0;

    public List<ItemVisuals> itemVisualsList = new List<ItemVisuals>();

    int curMaxSortInd = 0;

    public List<ItemBase> itemHoverList = new List<ItemBase>();

    public PickupModule currentlyHeldModule;
    
    private void Awake()
    {
        SetItemSortVisuals();
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

    void CheckForPour(ItemBase targetItem)
    {
        if (currentlyHeldModule != null)
        {
            var pourSource = currentlyHeldModule.GetComponent<PourModule>();
            var pourTarget = targetItem.GetComponent<PourTargetModule>();

            if (pourSource != null && pourTarget != null)
            {
                pourSource.SetStatePouring();
            }
        }
    }
    
    private void FixedUpdate()
    {
        
    }

    void UpdateHoverList()
    {
        if (itemHoverList.Count == 0) return;
        foreach (var v in itemHoverList) v.SetStateHover();
        var topHoverItem = itemHoverList.OrderByDescending(x => x.GetComponentInChildren<ItemVisuals>().sortInd).First();
        topHoverItem.SetStateTopHover();
        CheckForPour(topHoverItem);
    }

    void SetItemSortVisuals()
    {
        var l = GetComponentsInChildren<ItemVisuals>().ToList();

        foreach (var v in l)
        {
            v.SetSortingLayerIndex(curMaxSortInd);
            curMaxSortInd++;
        }
    }

    void SetItemMaskIndexes()
    {
        itemList = GetComponentsInChildren<Item2D>().ToList();
        foreach (var v in itemList)
        {
            v.GetComponent<MaskHandler>().SetMask(curMaskLayer);
            curMaskLayer++;
        }
       
    }
}
