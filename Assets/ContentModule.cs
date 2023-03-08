using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContentModule : MonoBehaviour
{
    public event Action<ContentModule, float> FluidOverflowEvent = (xxx, yyy) => { };

    TextMeshPro contentTM;

    public float curContents;
    public float maxContents;

    private void Awake()
    {
        contentTM = GetComponentInChildren<TextMeshPro>();
        ChangeContentAmount(0);
    }

    public void ChangeContentAmount(float x)
    {
        curContents += x;
        if (curContents > maxContents) FluidOverflowEvent(this, curContents - maxContents);
        curContents = Mathf.Clamp(curContents, 0, maxContents);
        contentTM.text = Mathf.Floor(curContents).ToString();
    }

   
}
