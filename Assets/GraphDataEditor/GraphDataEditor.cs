using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class GraphDataEditor : EditorWindow
{
    GraphDataEditorView dataEditorView;
    InspectorView inspectorView;

    [MenuItem("Graph/GraphDataEditor")]
    public static void OpenWindow()
    {
        GraphDataEditor wnd = GetWindow<GraphDataEditor>();
        wnd.titleContent = new GUIContent("GraphDataEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/GraphDataEditor/GraphDataEditor.uxml");
        visualTree.CloneTree(root);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/GraphDataEditor/GraphDataEditor.uss");
        root.styleSheets.Add(styleSheet);

        dataEditorView = root.Q<GraphDataEditorView>();
        inspectorView = root.Q<InspectorView>();
        dataEditorView.OnNodeSelected = OnNodeSelectionChanged;
        OnSelectionChange();
    }

    private void OnSelectionChange()
    {

        NodeHolder nodeHolder = Selection.activeObject as NodeHolder;
        if (nodeHolder && AssetDatabase.CanOpenAssetInEditor(nodeHolder.GetInstanceID()))
        {
            dataEditorView.PopulateView(nodeHolder);
        }
    }

    void OnNodeSelectionChanged(BaseNodeView node)
    {
        inspectorView.UpdateSelection(node);
    }
}