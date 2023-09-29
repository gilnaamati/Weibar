using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DrawerSystem
{
    public class DrawerCabinetView : GraphView
    {
        IMouseEvent _lastMouseDownEvent;
        private Vector2 mousePos;
        public Vector2 defaultNodeSize = new Vector2(150, 200);
        public DrawerCabinetView()
        {
            styleSheets.Add(Resources.Load<StyleSheet>("DrawerGraph"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContextualMenuManipulator(evt => BuildContextualMenu(evt)));

            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            GenerateEntryPointNode();
            this.RegisterCallback<PointerMoveEvent>(evt => mousePos = evt.localPosition);
        }
        
        // Port GeneratePort(DrawerNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            //Debug.Log(evt.mousePosition);
           // Debug.Log(evt.localMousePosition);
           // Debug.Log(evt.originalMousePosition);
            evt.menu.AppendAction("Create Node", (e) => { CreateNode(evt.localMousePosition); });
            evt.menu.AppendAction("Create Node22", (e) => { CreateNode(evt.mousePosition); });
        }
        
        
        private void GenerateEntryPointNode()
        {
            var n = new DrawerBaseNode
            {
                title = "Start",
                GUID = Guid.NewGuid().ToString(),
                EntryPoint = true
            };
            
            var outputPort = n.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
                typeof(float));
            n.outputContainer.Add(outputPort);
            outputPort.portName = "out";
            AddElement(n);
     
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach((port) =>
            {
                if (startPort != port && startPort.node != port.node)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }
        
        public void CreateNode(Vector3 pos, string nodeName = "Base Drawer Node")
        {
            Debug.Log(pos);
            var n = new DrawerBaseNode
            {
                title = nodeName,
                GUID = Guid.NewGuid().ToString()
            };

            var inputPort = n.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi,
                typeof(float));
            inputPort.portName = "in";
            n.inputContainer.Add(inputPort);
            var outputPort = n.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi,
                typeof(float));
            outputPort.portName = "out";
            n.outputContainer.Add(outputPort);

            //position the node where the mouse is
            n.SetPosition(new Rect
            {
                position = Vector2.zero,
                size = defaultNodeSize
            });
            
            
            pos = this.contentViewContainer.WorldToLocal( Input.mousePosition);
            pos = mousePos;
            n.RefreshPorts();
            n.RefreshExpandedState();
            n.SetPosition(new Rect
            {
                position = pos,
                size = defaultNodeSize
            });
            AddElement(n);
           
        }
    }
}


