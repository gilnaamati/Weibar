using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomerStateNode : DataNode
{
    public string StartDialogue;

    public override void Init(BaseNodeView _nodeView)
    {
        base.Init(_nodeView);

        nodeView.CreateInputPort("EnterKeyWords");
        nodeView.CreateOutputPort("ExitKeyWords");
    }
}
