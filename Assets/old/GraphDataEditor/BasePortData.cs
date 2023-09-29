using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class BaseEdgeData
{
    public BaseNode sourceNode;
    public string sourcePortName;
    public BaseNode targetNode;
    public string targetPortName;
}

[Serializable]
public class BasePortData
{
    [FormerlySerializedAs("PortName")] public string portName;
    public List<BaseEdgeData> edgeDataList = new List<BaseEdgeData>();
}
