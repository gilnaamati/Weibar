using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphViewSystem
{
    public class GraphSaveUtility
    {
        private DrawerGraphView _targetGraphView;
        private DrawerContainer _containerCache;

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
            Debug.Log("saving started");
            if (!Edges.Any()) return;
            Debug.Log("saving continued");
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
                });
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
            _containerCache = Resources.Load<DrawerContainer>(fileName);
            if (_containerCache == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "Where the heck is your thing", "mmmm");
                return;
            }

            ClearGraph();
            CreateNodes();
            ConnectNodes();
        }

        private void ConnectNodes()
        {
            for (var i = 0; i < Nodes.Count; i++)
            {
                var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == Nodes[i].GUID).ToList();
                for (var j = 0; j < connections.Count; j++)
                {
                    var targetNodeGuid = connections[j].TargetNodeGuid;
                    var targetNode = Nodes.First(x => x.GUID == targetNodeGuid);
                    LinkNodes(Nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);

                    targetNode.SetPosition(new Rect(

                        _containerCache.DrawerNodeDatas.First(x => x.NodeGUID == targetNodeGuid).Position,
                        _targetGraphView.defaultNodeSize
                    ));
                }
            }
        }

        private void LinkNodes(Port output, Port input)
        {
            var tempEdge = new Edge
            {
                output = output,
                input = input
            };
            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            _targetGraphView.Add(tempEdge);
        }

        private void CreateNodes()
        {
            foreach (var nodeData in _containerCache.DrawerNodeDatas)
            {
                var tempNode = _targetGraphView.CreateDrawerNode(nodeData.DrawerText);
                tempNode.GUID = nodeData.NodeGUID;
                _targetGraphView.AddElement(tempNode);

                var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == tempNode.GUID).ToList();
                nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName));
            }
        }

        private void ClearGraph()
        {
            Nodes.Find(x => x.EntryPoint).GUID = _containerCache.NodeLinks[0].BaseNodeGuid;

            foreach (var node in Nodes)
            {
                if (node.EntryPoint) continue;

                Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));

                _targetGraphView.RemoveElement(node);

            }
        }


    }
}