using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GraphSaveUtility
{
    DrawerGraphView _targetGraphView;
    List<Edge> Edges => _targetGraphView.edges.ToList();
    List<DrawerNode> Nodes => _targetGraphView.nodes.ToList().Cast<DrawerNode>().ToList();

    public static GraphSaveUtility GetInstance(DrawerGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string fileName)
    {
        if (!Edges.Any()) return;
      
        var drawerContainer = ScriptableObject.CreateInstance<DrawerContainer>();
        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();

        for (int i = 0; i < connectedPorts.Length; i++)
        {
         
            var outputNode = connectedPorts[i].output.node as DrawerNode;
            var inputNode = connectedPorts[i].input.node as DrawerNode;

            drawerContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            }) ;
        }

        foreach (var drawerNode in Nodes.Where(node => !node.EntryPoint))
        {
            drawerContainer.DrawerNodeDatas.Add(new DrawerNodeData
            {
                NodeGUID = drawerNode.GUID,
                DrawerText = drawerNode.DrawerText,
                Position = drawerNode.GetPosition().position
            });
        }

        AssetDatabase.CreateAsset(drawerContainer, $"Assets/GraphViewSystem/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
            
    }

    public void LoadGraph(string fileName)
    {

    }
}
