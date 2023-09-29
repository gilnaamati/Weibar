using System.Collections;
using System.Collections.Generic;
using GraphViewSystem;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DrawerSystem
{
    public class DrawerCabinet : GraphViewEditorWindow
    {
        private DrawerCabinetView _cabinetView;

        private string _fileName = "New Cabinet";
        
        [MenuItem("Graph/Cabinet  Graph")]
        public static void OpenDrawerGraphWindow()
        {
            var window = GetWindow<DrawerCabinet>();
            window.titleContent = new GUIContent(text: "Drawer Graph");
        }
            
        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
           // GenerateMinimap();
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();
            
            var fileNameTextField = new TextField("File Name: ");
            fileNameTextField.SetValueWithoutNotify(_fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
            toolbar.Add(fileNameTextField);
            
            var nodeCreateButton = new Button(clickEvent: () => { _cabinetView.CreateNode(Vector2.zero); });
            nodeCreateButton.text = "CreateNode";
            toolbar.Add(nodeCreateButton);

            rootVisualElement.Add(toolbar);
            
        }

        void ConstructGraphView()
        {
            _cabinetView = new DrawerCabinetView
            {
                name = "Drawer Graph"
            };

            _cabinetView.StretchToParentSize();
            rootVisualElement.Add(_cabinetView);
        }
    }
}
