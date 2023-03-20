using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DrawerGraphView : GraphView
{
    readonly Vector2 defaultNodeSize = new Vector2 (150, 200); 


    public DrawerGraphView()
    {
        styleSheets.Add(Resources.Load<StyleSheet>("DrawerGraph"));
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

       AddElement( GenerateEntryPointNode());
    }

    Port GeneratePort(DrawerNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, type: typeof(float));
    }

    private DrawerNode GenerateEntryPointNode()
    {
        var node = new DrawerNode
        {
            title = "start",
            GUID = Guid.NewGuid().ToString(),
            DrawerText = "EntryPoint",
            EntryPoint = true
        };



        var newPort = GeneratePort(node, Direction.Output);
        newPort.portName = "Next";
        node.outputContainer.Add(newPort);

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(x:100, y:200, width: 100, height: 150));
        return node;
    }

    public void CreateNode(string nodeName)
    {
        AddElement(CreateDrawerNode(nodeName));
    }

    public override List<Port> GetCompatiblePorts( Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach((port) =>
       {
           if (startPort != port && startPort.node != port.node)
           {
               compatiblePorts.Add(port);
           }
       });

        return compatiblePorts;
    }

    public DrawerNode CreateDrawerNode(string nodeName)
    {
        var drawerNode = new DrawerNode
        {
            title = nodeName,
            DrawerText = nodeName,
            GUID = Guid.NewGuid().ToString()
        };

        var inputPort = GeneratePort(drawerNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        drawerNode.inputContainer.Add(inputPort);

        var button = new Button(clickEvent: ()=> { AddChoicePort(drawerNode); });
        button.text = "New Choice";

        drawerNode.titleContainer.Add(button);

        drawerNode.RefreshExpandedState();
        drawerNode.RefreshPorts();
        drawerNode.SetPosition(new Rect(Vector2.zero,defaultNodeSize));

        return drawerNode;
        

    }

    public Port AddChoicePort(DrawerNode drawerNode, string overridenPortName = "")
    {
        var p =  GeneratePort(drawerNode, Direction.Output );

      //  var oldLabel = p.contentContainer.Q<Label>("type");
      //  p.contentContainer.Remove(oldLabel);
        
        var outputPortCount = drawerNode.outputContainer.Query(name: "connector").ToList().Count;
        var choicePortName = string.IsNullOrEmpty(overridenPortName) ? $"Choice {outputPortCount}" : overridenPortName;

        var textField = new TextField
        {
            name = string.Empty,
            value = choicePortName
        };

        textField.RegisterValueChangedCallback(evt => p.portName = evt.newValue);
        p.contentContainer.Add(new Label (" "));
        p.contentContainer.Add(textField);
        var deleteButton = new Button(clickEvent: () => RemovePort(drawerNode, p))
        {
            text = "X"
        };
        p.contentContainer.Add(deleteButton);
        
        p.portName = choicePortName;
        drawerNode.outputContainer.Add(p);
        drawerNode.RefreshExpandedState();
        drawerNode.RefreshPorts();
        return p;
    }

    private void RemovePort(DrawerNode drawerNode, Port p)
    {
        var targetEdge = edges.ToList().Where(x => x.output.portName == p.portName && x.output.node == p.node);

        if (targetEdge.Any())
        {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }

        drawerNode.outputContainer.Remove(p);
        drawerNode.RefreshPorts();
        drawerNode.RefreshExpandedState();

    }
}
