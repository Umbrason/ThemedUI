using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ThemedImage)), CanEditMultipleObjects]
public class ThemedImageEditor : Editor
{
    private const float cellSize = 25f;
    SerializedProperty paletteProperty;
    private ThemedUIPalette palette { get { return paletteProperty.objectReferenceValue as ThemedUIPalette; } }
    SerializedProperty colorIndexProperty;
    SerializedProperty typeProperty;

    void OnEnable()
    {
        colorIndexProperty = serializedObject.FindProperty("colorIndex");
        typeProperty = serializedObject.FindProperty("m_Type");
        paletteProperty = serializedObject.FindProperty("palette");
    }

    public override void OnInspectorGUI()
    {
        if (serializedObject == null)
            return;
        if (!palette)
            paletteProperty.objectReferenceValue = ThemedUIEditorUtility.ActivePalette;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Sprite"), new GUIContent("Source Image"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("palette"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Color"));
        EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_RaycastTarget"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_RaycastPadding"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Maskable"));
        EditorGUILayout.PropertyField(typeProperty);


        EditorGUI.indentLevel = 1;
        switch (typeProperty.intValue)
        {
            //Simple
            case 0:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_UseSpriteMesh"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_PreserveAspect"));
                SetNativeSizeButton();
                break;
            //Filled
            case 3:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_FillMethod"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_FillOrigin"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_FillAmount"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_FillClockwise"), new GUIContent("Clockwise"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_PreserveAspect"));
                SetNativeSizeButton();
                break;
            //Sliced or Tiled
            default:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_FillCenter"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_PixelsPerUnitMultiplier"));
                break;
        }

        EditorGUI.indentLevel = 0;
        //Material m = (Material)serializedObject.FindProperty("m_Material").objectReferenceValue;        
        EditorGUILayout.Separator();
        ThemedUIEditorUtility.DrawColorButtons(palette, ref colorIndexProperty, cellSize);
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();
    }

    private void SetNativeSizeButton()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUI.indentLevel = 0;
        GUILayout.Space(EditorGUIUtility.labelWidth);
        if (GUILayout.Button("Set Native Size"))
            (target as ThemedImage).SetNativeSize();
        EditorGUI.indentLevel = 1;
        EditorGUILayout.EndHorizontal();
    }

}