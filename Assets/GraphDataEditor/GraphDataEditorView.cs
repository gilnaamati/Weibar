using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

public class GraphDataEditorView : GraphView
{
    public Action<BaseNodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<GraphDataEditorView, GraphView.UxmlTraits> { };
    GraphDataEditor graphDataEditor;
    NodeHolder nodeHolder;

    public GraphDataEditorView()
    {
        Insert(0, new GridBackground());
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/GraphDataEditor/GraphDataEditor.uss");
        styleSheets.Add(styleSheet);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new ContentZoomer());
    }

    internal void PopulateView(NodeHolder nodeHolder)
    {
        this.nodeHolder = nodeHolder;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;
        
        nodeHolder.nodes.ForEach(n => CreateNodeView(n));

        nodeHolder.nodes.ForEach(n =>
        {
            var portDataList = n.outputPortList;
            var parentView = FindNodeView(n);
            portDataList.ForEach(p =>
            {
                var portEdgeList = p.edgeDataList;
                foreach (var e in portEdgeList)
                {
                    var l = parentView.outputPorts.Where(x => x.portName == p.portName).ToList();
                    var outputPort = parentView.outputPorts.First(x => x.portName == p.portName);
                    var childView = FindNodeView(e.targetNode);
                    var inputPort = childView.inputPorts.First(x => x.portName == e.targetPortName);
                    var edge = outputPort.ConnectTo(inputPort);
                    AddElement(edge);
                }
            });
        });
    }

    BaseNodeView FindNodeView(BaseNode node)
    {
        return GetNodeByGuid(node.guid) as BaseNodeView;
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(elem =>
            {
                BaseNodeView nodeView = elem as BaseNodeView;
                if (nodeView != null)
                {
                    nodeHolder.DeleteNode(nodeView.node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    BaseNodeView parentView = edge.output.node as BaseNodeView;
                    BaseNodeView childView = edge.input.node as BaseNodeView;
                    string parentPortName = edge.output.portName;
                    string childPortName = edge.input.portName;
                    nodeHolder.RemoveChild(parentView.node, childView.node, parentPortName, childPortName);
                }
            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                BaseNodeView parentView = edge.output.node as BaseNodeView;
                BaseNodeView childView = edge.input.node as BaseNodeView;
                string parentPortName = edge.output.portName;
                string childPortName = edge.input.portName;
                nodeHolder.AddChild(parentView.node, childView.node, parentPortName, childPortName);
            });
        }
        return graphViewChange;
    }
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);
        {
            var types = TypeCache.GetTypesDerivedFrom<DataNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] /{type.Name}", (a) => CreateNode(type));
            }
        }
    }

    void CreateNode(System.Type type)
    {
        BaseNode node = nodeHolder.CreateNode(type);
        node.Init();
        CreateNodeView(node);
    }

    private BaseNodeView CreateNodeView(BaseNode node)
    {
        BaseNodeView nodeView = new BaseNodeView(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        foreach (var v in node.inputPortList)
        {
            nodeView.CreateInputPort(v.portName);
        }
        foreach (var v in node.outputPortList)
        {
            nodeView.CreateOutputPort(v.portName);
        }
       
        //get all the string variables in the node
        //and create a text field for each one

        var fields = node.GetType().GetFields();
        foreach (var field in fields)
        {
            if (field.FieldType == typeof(string) || field.FieldType == typeof(int) || field.FieldType == typeof(float))
            {
                //if they are not hidden in inspector
                if (!Attribute.IsDefined(field, typeof(HideInInspector)))
                nodeView.CreateInputText(field.Name);
            }
        }
       
       

        AddElement(nodeView);
        return nodeView;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();

    }
}
