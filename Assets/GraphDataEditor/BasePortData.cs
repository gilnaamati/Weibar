using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public string PortName;
    public List<BaseEdgeData> edgeDataList = new List<BaseEdgeData>();
}
