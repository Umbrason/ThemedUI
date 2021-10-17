using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ThemedText)), CanEditMultipleObjects]
public class ThemedTextEditor : Editor
{
    private const float cellSize = 25f;
    SerializedProperty paletteProperty;
    private ThemedUIPalette palette { get { return paletteProperty.objectReferenceValue as ThemedUIPalette; } }
    SerializedProperty colorIndexProperty;

    void OnEnable()
    {
        colorIndexProperty = serializedObject.FindProperty("colorIndex");
        paletteProperty = serializedObject.FindProperty("palette");
    }

    public override void OnInspectorGUI()
    {
        if (serializedObject == null)
            return;
        if (!palette)
            paletteProperty.objectReferenceValue = ThemedUIEditorUtility.ActivePalette;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Text"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("palette"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Color"));
        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_FontData"));
        EditorGUILayout.Separator();
        ThemedUIEditorUtility.DrawColorButtons(palette, ref colorIndexProperty, cellSize);
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }
}