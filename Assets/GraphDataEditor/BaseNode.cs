using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public abstract class BaseNode : ScriptableObject
{
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
    protected BaseNodeView nodeView;

    public virtual void Init(BaseNodeView _nodeView)
    {
        nodeView = _nodeView;
    }
   
   
}
