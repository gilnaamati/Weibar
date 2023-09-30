using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UrgeThreshold
{
    public float max, min;
    public string thresholdName;
}

public class CustUrge : MonoBehaviour
{
    public event Action MaxOutEvent = () => { };
    public event Action DepletedEvent = () => { };
    public event Action<UrgeThreshold> EnterThresholdEvent = (x) => { };
    public event Action<UrgeThreshold> ExitThresholdEvent = (x) => { };

    public float start = 10;
    public float max = 100;
    public float min = 0;
    public float increaseSpeed = 10f;
    public UrgeMeterBar bar;
    public string urgeName;
    public string barLetter;
    public float curCount;
    bool maxedOut;
    bool depleted;
    public List<UrgeThreshold> urgeThresholdList = new List<UrgeThreshold>();
    public List<UrgeThreshold> activeThresholdList = new List<UrgeThreshold>();


    private void Awake()
    {
        curCount = start;
        UpdateBar();
    }

    public void SetUrge (float f)
    {
        curCount = f;
        UpdateBar();
    }

    


    private void Update()
    {
        ChangeUrgeAmount (increaseSpeed * Time.deltaTime);
    }

    public void ChangeUrgeAmount(float a)
    {
        curCount += a;

        if (curCount >= max)
        {
            if (!maxedOut) HitMax();
            curCount = max;
        }
        else
        {
            maxedOut = false;
        }

        if (curCount <= min)
        {
            if (!depleted) HitMin();
            curCount = min;
        }
        else
        {
            depleted = false;
        }

        CheckThresholds();
        UpdateBar();
    }

    void CheckThresholds()
    {
        foreach (var v in urgeThresholdList)
        {
            if (curCount >= v.min && curCount <= v.max)
            {
                if (!activeThresholdList.Contains(v))
                {
                    activeThresholdList.Add(v);
                    Debug.Log(v.thresholdName + " added");
                    EnterThresholdEvent(v);
                }
            }
        }

        var l = new List<UrgeThreshold>();

        foreach (var v in activeThresholdList)
        {
            if (curCount < v.min || curCount > v.max)
            {
                l.Add(v);
                Debug.Log(v.thresholdName + " removed");
                ExitThresholdEvent(v);
            }
        }

        foreach (var v in l)
        {
            activeThresholdList.Remove(v);
        }
    }

    void UpdateBar()
    {
        bar.SetBar(curCount / max);
    }

    void HitMax()
    {
        maxedOut = true;
        MaxOutEvent();
    }

    void HitMin()
    {
        depleted = true;
        DepletedEvent();
    }

}
