using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ContentModule : MonoBehaviour
{
    public event Action<ContentModule, float> FluidOverflowEvent = (xxx, yyy) => { };

    public List<Transform> itemCornerList = new List<Transform>();
    public Transform liquidVisuals;
    TextMeshPro contentTM;

    public float curContents;
    public float maxContents;

    private void Awake()
    {
        contentTM = GetComponentInChildren<TextMeshPro>();
        ChangeContentAmount(0);
    }

    private void FixedUpdate()
    {
        UpdateVisuals();
    }

    public void ChangeContentAmount(float x)
    {
        curContents += x;
        if (curContents > maxContents) FluidOverflowEvent(this, curContents - maxContents);
        curContents = Mathf.Clamp(curContents, 0, maxContents);
        contentTM.text = Mathf.Floor(curContents).ToString();
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        var x = itemCornerList.Average(x => x.position.x);
        var maxY = itemCornerList.Max(x => x.position.y);
        var minY = itemCornerList.Min(x => x.position.y);
        var y = Mathf.Lerp(minY, maxY, curContents / maxContents);
        liquidVisuals.position = new Vector3(x, y, liquidVisuals.position.z);

    }

   
}
