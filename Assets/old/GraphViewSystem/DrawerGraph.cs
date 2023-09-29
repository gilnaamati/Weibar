using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphViewSystem
{
    public class DrawerGraph : GraphViewEditorWindow
    {


        DrawerGraphView _graphView;
        string _fileName = "New Cabinet";



        [MenuItem("Graph/Drawer Graph")]
        public static void OpenDrawerGraphWindow()
        {
            var window = GetWindow<DrawerGraph>();
            window.titleContent = new GUIContent(text: "Drawer Graph");

        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMinimap();
        }

        private void GenerateMinimap()
        {
            var miniMap = new MiniMap { anchored = true };
            miniMap.SetPosition(new Rect(10, 40, 200, 140));
            _graphView.Add(miniMap);

        }

        void ConstructGraphView()
        {
            _graphView = new DrawerGraphView
            {
                name = "Drawer Graph"
            };

            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            var fileNameTextField = new TextField("File Name: ");
            fileNameTextField.SetValueWithoutNotify(_fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
            toolbar.Add(fileNameTextField);

            toolbar.Add(new Button(clickEvent: () => RequestDataOperation(true)) { text = "Save Data" });
            toolbar.Add(new Button(clickEvent: () => RequestDataOperation(false)) { text = "Load Data" });

            var nodeCreateButton = new Button(clickEvent: () => { _graphView.CreateNode("Drawer Node"); });
            nodeCreateButton.text = "CreateNode";
            toolbar.Add(nodeCreateButton);

            rootVisualElement.Add(toolbar);
        }

        private void RequestDataOperation(bool save)
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                EditorUtility.DisplayDialog("Invalid file name", "please enter a vald file name", "right-o");
            }

            var saveUtility = GraphSaveUtility.GetInstance(_graphView);

            if (save)
                saveUtility.SaveGraph(_fileName);
            else
                saveUtility.LoadGraph(_fileName);
        }


        private void SaveData()
        {
            throw new NotImplementedException();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }
    }
}

