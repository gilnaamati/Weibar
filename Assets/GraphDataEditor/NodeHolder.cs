using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
[CreateAssetMenu()]
public class NodeHolder : ScriptableObject
{
    public List<BaseNode> nodes = new List<BaseNode>();
    // Start is called before the first frame update

    public BaseNode CreateNode(System.Type type)
    {
        BaseNode node = ScriptableObject.CreateInstance(type) as BaseNode;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);
        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(BaseNode node)
    {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(BaseNode parent, BaseNode child, string parentPortName, string childPortName)
    {
        BaseEdgeData edgeData = new BaseEdgeData
        {
            sourceNode = parent,
            targetNode = child,
            sourcePortName = parentPortName,
            targetPortName = childPortName
        };

        var parentPortData = parent.outputPortList.First(x => x.PortName == parentPortName);
        if (parentPortData == null) Debug.LogError("Can't find parent port!");
        else parentPortData.edgeDataList.Add(edgeData);

        var childPortData = child.inputPortList.First(x => x.PortName == childPortName);
        if (childPortName == null) Debug.LogError("Can't find child port!");
        else childPortData.edgeDataList.Add(edgeData);
    }

    public void RemoveChild(BaseNode parent, BaseNode child, string parentPortName, string childPortName)
    {
        var parentPortData = parent.outputPortList.First(x => x.PortName == parentPortName);
        if (parentPortData == null)
        {
            Debug.LogError("Can't find parent port!");
            return;
        }
        var childPortData = child.inputPortList.First(x => x.PortName == childPortName);
        if (childPortName == null)
        {
            Debug.LogError("Can't find child port!");
            return;
        }
 
        BaseEdgeData edgeData = parentPortData.edgeDataList.First(x => x.targetNode ==
         child && x.targetPortName == childPortName);
        if (edgeData == null)
        {
            Debug.LogError("Can't find edgeData!");
            return;
        }

        parentPortData.edgeDataList.Remove(edgeData);
        childPortData.edgeDataList.Remove(edgeData);
    }

    public List<BaseNode> GetChildren(BaseNode parent)
    {
        List<BaseNode> children = new List<BaseNode>();
       

        return children;
    }

   
}
