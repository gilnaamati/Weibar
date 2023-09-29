using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMdata : MonoBehaviour
{
    public float var1;
    public CMPosition posCM;
    public enum BodyState
    {
        Standing,
        Sitting,
        GettingUp,
        SittingDown
    };

    public BodyState bodyState = BodyState.Standing;

    public PosMarker posMarker;

    public List<TestItem> itemHoldingList = new List<TestItem>();

    private void Awake()
    {
        posMarker = GetComponentInChildren<PosMarker>();
    }

}
