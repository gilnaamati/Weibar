using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public List<Item2D> itemList = new List<Item2D>();

    int curMaskLayer = 0;

    private void Awake()
    {
        itemList = GetComponentsInChildren<Item2D>().ToList();
        foreach (var v in itemList)
        {
            v.GetComponent<MaskHandler>().SetMask(curMaskLayer);
            curMaskLayer++;
        }

    }
}
