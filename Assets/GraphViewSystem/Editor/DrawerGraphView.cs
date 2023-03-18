using System;
using System.Collections.Generic;
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

    private Port AddChoicePort(DrawerNode drawerNode)
    {
        var p =  GeneratePort(drawerNode, Direction.Output );
        var outputPortCount = drawerNode.outputContainer.Query(name: "connector").ToList().Count;
        var outputPortName = $"Choice {outputPortCount}";
        p.portName = outputPortName;
        drawerNode.outputContainer.Add(p);
        drawerNode.RefreshExpandedState();
        drawerNode.RefreshPorts();
        return p;
    }
}
