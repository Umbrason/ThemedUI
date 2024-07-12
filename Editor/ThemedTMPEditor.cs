using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

[CustomEditor(typeof(ThemedTextMeshPro)), CanEditMultipleObjects]
public class ThemedTMPEditor : TMPro.EditorUtilities.TMP_EditorPanelUI
{
    private const float cellSize = 25f;
    SerializedProperty paletteProperty;
    private ThemedUIPalette palette { get { return paletteProperty.objectReferenceValue as ThemedUIPalette; } }
    SerializedProperty colorIndexProperty;

    new void OnEnable()
    {
        base.OnEnable();
        colorIndexProperty = serializedObject.FindProperty("colorIndex");
        paletteProperty = serializedObject.FindProperty("palette");
    }


    public override void OnInspectorGUI()
    {
        if (serializedObject == null)
            return;
        if (!palette)
            paletteProperty.objectReferenceValue = ThemedUIEditorUtility.ActivePalette;        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("palette"));
        ThemedUIEditorUtility.DrawColorButtons(palette, ref colorIndexProperty, cellSize);
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Separator();
        base.OnInspectorGUI();

    }
}
