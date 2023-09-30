using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourTargetModule : MonoBehaviour
{
    ContentModule cm;

    public enum PourTargetState
    {
        Idle,
        PouredInto
    }

    public PourTargetState pourTargetState;

    private void Awake()
    {
        cm = GetComponent<ContentModule>();
    }

    // Start is called before the first frame update
    public void RecieveLiquids(List<ContentPart> receiveList)
    {
        if (pourTargetState == PourTargetState.Idle) pourTargetState = PourTargetState.PouredInto;   
        cm.AddContents(receiveList);
    }

    public void SetStateIdle()
    {
        pourTargetState = PourTargetState.Idle;
    }
}
