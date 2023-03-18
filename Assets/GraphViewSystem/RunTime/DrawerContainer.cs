using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DrawerContainer : ScriptableObject
{
    public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
    public List<DrawerNodeData> DrawerNodeDatas = new List<DrawerNodeData>();
}
