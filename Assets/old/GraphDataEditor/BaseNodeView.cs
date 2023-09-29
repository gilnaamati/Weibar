using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseNodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<BaseNodeView> OnNodeSelected;
    public BaseNode node;
    public List<Port> inputPorts = new List<Port>();
    public List<Port> outputPorts = new List<Port>();

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
        inputPorts.Add(input);
        inputContainer.Add(input);
    }

    public void CreateOutputPort(string portName, Port.Capacity cap = Port.Capacity.Multi)
    {
        var output = InstantiatePort(Orientation.Horizontal, Direction.Output, cap, typeof(bool));
        output.portName = portName;
        outputPorts.Add(output);
        outputContainer.Add(output);
    }

    public void CreateInputText(string textFieldName)
    {
        var textField = new TextField
        {
            multiline = true,
            name = textFieldName,
            value = "Write here",
            label = textFieldName
            
        };
        //make text background dark gray
        contentContainer.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
        contentContainer.Add(textField);
       
        //if this is a connectorNode, make the text field the title
        if (node is ConnectorNode)
        {
            textField.RegisterValueChangedCallback(evt => title = evt.newValue);
        }
        
        textField.RegisterValueChangedCallback(evt => node.UpdateField(textFieldName, evt.newValue));
        RefreshExpandedState();
        RefreshPorts();
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
