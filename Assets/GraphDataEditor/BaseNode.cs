using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;




public abstract class BaseNode : ScriptableObject
{
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
   // protected BaseNodeView nodeView;
    public List<BasePortData> inputPortList = new List<BasePortData>();
    public List<BasePortData> outputPortList = new List<BasePortData>();
    public virtual void Init()
    {
       // nodeView = _nodeView;
    }

    public virtual void CreateInputPort (string portName)
    {
       // nodeView.CreateInputPort(portName);
        inputPortList.Add(new BasePortData
        {
            PortName = portName
        });
    }

    public virtual void CreateOutputPort(string portName)
    {
       // nodeView.CreateOutputPort(portName);
        outputPortList.Add(new BasePortData
        {
            PortName = portName
        });
    }


}
