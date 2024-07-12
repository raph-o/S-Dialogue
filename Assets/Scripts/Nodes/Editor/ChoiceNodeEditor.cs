using UnityEngine;
using XNode;
using XNodeEditor;

[CustomNodeEditor(typeof(ChoiceNode))]
public class ChoiceNodeEditor : NodeEditor
{
    public override void OnBodyGUI()
    {
        serializedObject.Update();

        ChoiceNode node = target as ChoiceNode;
        NodeEditorGUILayout.PortField(node.GetPort("entry"));
        
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("speaker"), new GUIContent("Speaker"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("text"), new GUIContent("Text"));
        NodeEditorGUILayout.DynamicPortList("choices", typeof(BaseNode), serializedObject, NodePort.IO.Output, Node.ConnectionType.Override);
        
        serializedObject.ApplyModifiedProperties();
    }
}