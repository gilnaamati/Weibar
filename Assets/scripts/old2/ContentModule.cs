using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[Serializable]
public class ContentPart
{
    public LiquidData liquid;
    public float amount;

    public ContentPart(float a, LiquidData ld)
    {
        liquid = ld;
        amount = a;
    }
}

public class ContentModule : MonoBehaviour
{
    public event Action<ContentModule, List<ContentPart>> FluidOverflowEvent = (xxx, yyy) => { };

    public event Action ContainerEmptyEvent = () => { };

    public List<ContentPart> contentList = new List<ContentPart>();

    public List<Transform> itemCornerList = new List<Transform>();
    public Transform liquidVisuals;
    TextMeshPro contentTM;

    public Color curContentsColor;

    public float curConAm;
    public float maxContents;

    private void Awake()
    {
        contentTM = GetComponentInChildren<TextMeshPro>();
        UpdateCurContents();
    }

    private void FixedUpdate()
    {
        UpdateVisuals();
    }

    public List<ContentPart> RemoveContents(float a)
    {
        var r = a / curConAm;
        var contentRemovalList = new List<ContentPart>();
        foreach (var v in contentList)
        {
            var removeAmount = v.amount * r;
            v.amount -= removeAmount;
            contentRemovalList.Add(new ContentPart(removeAmount, v.liquid));
        }
        UpdateCurContents();
        return contentRemovalList;
    }

    public void AddContents( List<ContentPart> contentsAdded)
    {
        foreach (var v in contentsAdded) contentList.Add(v);
        ConsolidateContents();
        UpdateCurContents();
    }

    void UpdateCurContents()
    {
        curConAm = contentList.Sum(x => x.amount);
        if (curConAm > maxContents)
        {
            var overflowcontents = RemoveContents(curConAm - maxContents);
            FluidOverflowEvent(this, overflowcontents);
        }
        contentTM.text = Mathf.Floor(curConAm).ToString();
        UpdateVisuals();
    }


    void ConsolidateContents()
    {
        List<ContentPart> newContentList = new List<ContentPart>();
        foreach (var v in contentList)
        {
            if (newContentList.Count(x=>x.liquid == v.liquid) == 0)
            {
                var partsSum = contentList.Where(x => x.liquid == v.liquid).Sum(x => x.amount);
                newContentList.Add(new ContentPart(partsSum, v.liquid));
            }
        }
        contentList = newContentList.ToList();
    }

    public void UpdateVisuals()
    {
        var x = itemCornerList.Average(x => x.position.x);
        var maxY = itemCornerList.Max(x => x.position.y);
        var minY = itemCornerList.Min(x => x.position.y);
        var y = Mathf.Lerp(minY, maxY, curConAm / maxContents);
        liquidVisuals.position = new Vector3(x, y, liquidVisuals.position.z);
        UpdateContentsColor();

    }

    void UpdateContentsColor()
    {
        curContentsColor = new Color(0, 0, 0, 0);

        foreach (var v in contentList)
        {
            var r = (v.amount / curConAm);
            curContentsColor.a += v.liquid.liquidColor.a * r ;
            curContentsColor.r += v.liquid.liquidColor.r * r ;
            curContentsColor.b += v.liquid.liquidColor.b * r;
            curContentsColor.g += v.liquid.liquidColor.g * r;
        }

        var l = liquidVisuals.gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (var v in l) v.color = curContentsColor;
    }


   
}
