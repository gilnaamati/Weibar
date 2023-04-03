using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

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

        //  if (nodeHolder.rootNode == null)
        //  {
        //       tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
        //       EditorUtility.SetDirty(tree);
        //       AssetDatabase.SaveAssets();
        //    }

        nodeHolder.nodes.ForEach(n => CreateNodeView(n));

        nodeHolder.nodes.ForEach(n =>
        {
            var portDataList = n.outputPortList;
          
            portDataList.ForEach(p =>
            {
                var parentView = FindNodeView(n);
                var portEdgeList = p.edgeDataList;
                foreach (var e in portEdgeList)
                {
                    var outputPort = parentView.outputPorts.First(x => x.portName == e.sourcePortName);
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
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
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
            nodeView.CreateInputPort(v.PortName);
        }
        foreach (var v in node.outputPortList)
        {
            nodeView.CreateOutputPort(v.PortName);
        }

        AddElement(nodeView);
        return nodeView;
    }
}
