using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

[CustomEditor(typeof(ThemedTextMeshPro)),CanEditMultipleObjects]
public class ThemedTMPEditor : TMPro.EditorUtilities.TMP_EditorPanelUI
{
    private const float cellSize = 25f;
    private ThemedUIPalette palette { get { return (target as ThemedTextMeshPro)?.palette; } }
    SerializedProperty colorIndexProperty;

    new void OnEnable()
    {
        base.OnEnable();
        colorIndexProperty = serializedObject.FindProperty("colorIndex");
    }
    

    public override void OnInspectorGUI()
    {
        if (serializedObject == null)
            return;
        if (GUILayout.Button("DebugMesh"))
        {
            Vector2[] uvs = (target as ThemedTextMeshPro).mesh.uv;
            foreach (Vector2 uv in uvs)
                Debug.Log(uv);
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("palette"));
        ThemedUIEditorUtility.DrawColorButtons(palette, ref colorIndexProperty, cellSize);
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();        
        EditorGUILayout.Separator();
        base.OnInspectorGUI();

    }
}
