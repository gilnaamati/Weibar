using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomerStateTalkNode : CustomerStateNode
{
    public string StartDialogue;
    public float StartDialogueDelay;
    public override void Init()
    {
        base.Init();
        CreateInputPort("EnterKeyWords");
        CreateOutputPort("ExitKeyWords");
     
    }
}
