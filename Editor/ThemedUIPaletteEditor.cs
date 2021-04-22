using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ThemedUIPalette))]
public class ThemedUIPaletteEditor : Editor
{
    private UnityEditorInternal.ReorderableList list;
    private SerializedProperty colorProperty;
    void OnEnable()
    {
        InitializeSerializedObjects();
        UpdateSerializedObjects();

        colorProperty = serializedObject.FindProperty("colors");
        list = new UnityEditorInternal.ReorderableList(serializedObject, colorProperty);
        list.drawElementCallback = DrawListItems;
        list.drawHeaderCallback = DrawHeader;
        list.onReorderCallback = (x) => UpdateSerializedObjects();
    }

    private void InitializeSerializedObjects()
    {
        foreach (Object target in targets)
        {
            ThemedUIPalette palette = target as ThemedUIPalette;
            palette.InitializeTexture();
            palette.InitializeMaterial();
            palette.InitializeMaterial_Font();
            palette.InitializeMaterial_TMPFont();
        }
    }

    private void UpdateSerializedObjects()
    {
        foreach (Object target in targets)
        {
            ThemedUIPalette palette = target as ThemedUIPalette;
            palette.UpdateTexture();
            palette.UpdateMaterial();
            palette.UpdateMaterial_Font();
            palette.UpdateMaterial_TMPFont();
        }
    }

    public override void OnInspectorGUI()
    {
        if (serializedObject == null)
            return;
        serializedObject.Update();
        list.DoLayoutList();
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("material"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("material_Font"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("texture"));
        if (serializedObject.hasModifiedProperties)
        {
            serializedObject.ApplyModifiedProperties();            
            UpdateSerializedObjects();
        }
    }



    private void DrawHeader(Rect rect) => EditorGUI.LabelField(rect, "Colors");
    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {

        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
        EditorGUI.PropertyField(rect, element);
    }
}