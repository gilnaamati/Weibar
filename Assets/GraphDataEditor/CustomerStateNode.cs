using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomerStateNode : DataNode
{
    public string StartDialogue;

    public override void Init()
    {
        base.Init();
        CreateInputPort("EnterKeyWords");
        CreateOutputPort("ExitKeyWords");
    }
}
