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

    private void Awake()
    {
        SetItemSortVisuals();
        ItemBase.HoverEnterEvent += ItemBase_HoverEnterEvent;
        ItemBase.HoverExitEvent += ItemBase_HoverExitEvent;

    }

    private void ItemBase_HoverExitEvent(ItemBase obj)
    {
        if (itemHoverList.Contains(obj))
        {
            itemHoverList.Remove(obj);
            obj.SetStateNoHover();
        }
    }
    private void ItemBase_HoverEnterEvent(ItemBase obj)
    {
        if (!itemHoverList.Contains(obj))
        {
            itemHoverList.Add(obj);
            obj.SetStateHover();
        }   
    }

    private void FixedUpdate()
    {
        UpdateHoverList();
    }

    void UpdateHoverList()
    {
        if (itemHoverList.Count == 0) return;
        foreach (var v in itemHoverList) v.SetStateHover();
        var topHoverItem = itemHoverList.OrderByDescending(x => x.GetComponentInChildren<ItemVisuals>().sortInd).First();
        topHoverItem.SetStateTopHover();
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
