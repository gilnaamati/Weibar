using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BaseNodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<BaseNodeView> OnNodeSelected;
    public BaseNode node;
    public List<Port> inputPorts;
    public List<Port> outputPorts;

    public BaseNodeView(BaseNode node)
    {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.guid;
        style.left = node.position.x;
        style.top = node.position.y;
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
    }

    public void CreateInputPort(string portName, Port.Capacity cap = Port.Capacity.Multi)
    {
        var input = InstantiatePort(Orientation.Horizontal, Direction.Input, cap, typeof(bool));
        input.portName = portName;
        inputContainer.Add(input);
    }

    public void CreateOutputPort(string portName, Port.Capacity cap = Port.Capacity.Multi)
    {
        var output = InstantiatePort(Orientation.Horizontal, Direction.Output, cap, typeof(bool));
        output.portName = portName;
        outputContainer.Add(output);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (OnNodeSelected != null)
        {
            OnNodeSelected.Invoke(this);
        }
    }
}
