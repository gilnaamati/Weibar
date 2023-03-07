using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContentModule : MonoBehaviour
{
    TextMeshPro contentTM;

    public float curContents;
    public float maxContents;

    private void Awake()
    {
        contentTM = GetComponentInChildren<TextMeshPro>();
    }

    public void ChangeContentAmount(float x)
    {
        curContents += x;
        curContents = Mathf.Clamp(curContents, 0, maxContents);
        contentTM.text = Mathf.Floor(curContents).ToString();
    }
}
