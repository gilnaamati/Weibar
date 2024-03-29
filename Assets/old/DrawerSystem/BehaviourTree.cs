using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    public Node rootNode;
    public Node.State treeState = Node.State.Running;
    public List<Node> nodes = new List<Node>();
    public Node.State Update()
    {
        if (rootNode.state == Node.State.Running)
        {
            treeState = rootNode.Update();
        }
        return treeState;
       
    }

    public Node CreateNode (System.Type type)
    {
        Node node = ScriptableObject.CreateInstance(type) as Node;
        node.name = type.Name;
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);
        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
        return node;

    }

    public void DeleteNode(Node node)
    {
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(Node parent, Node child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator != null)
        {
            decorator.child = child;
        }
        
        RootNode rootNode = parent as RootNode;
        if (rootNode != null)
        {
            rootNode.child = child;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite != null)
        {
            composite.children.Add( child);
        }
        

    }

    public void RemoveChild(Node parent, Node child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator != null)
        {
            decorator.child = null;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite != null)
        {
            composite.children.Remove(child);
        }
        
        RootNode rootNode = parent as RootNode;
        if (rootNode != null)
        {
            rootNode.child = null;
        }
    }

    public List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator != null)
        {
            if (decorator.child != null)
                children.Add(decorator.child);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite != null)
        {
            return composite.children.ToList();
        }
        
        RootNode rootNode = parent as RootNode;
        if (rootNode != null)
        {
            if (rootNode.child != null)
                children.Add(rootNode.child);
        }

        return children;
    }

    public BehaviourTree Clone()
    {
        var tree = Instantiate(this);
        tree.rootNode = tree.rootNode.Clone();
        return tree;
    }
}
