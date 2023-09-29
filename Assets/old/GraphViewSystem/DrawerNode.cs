using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GraphViewSystem
{
    public class DrawerNode : UnityEditor.Experimental.GraphView.Node
    {
        public string GUID;

        public string DrawerText;

        public bool EntryPoint;
    }
}
