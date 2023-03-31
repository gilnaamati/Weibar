using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CreateAssetMenu()]
public class NodeHolder : MonoBehaviour
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

    public void AddChild(BaseNode parent, BaseNode child)
    {
       

    }

    public void RemoveChild(BaseNode parent, BaseNode child)
    {
     
    }

    public List<BaseNode> GetChildren(BaseNode parent)
    {
        List<BaseNode> children = new List<BaseNode>();
       

        return children;
    }
}
