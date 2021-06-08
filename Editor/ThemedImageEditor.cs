using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ThemedImage)), CanEditMultipleObjects]
public class ThemedImageEditor : Editor
{
    private const float cellSize = 25f;
    private ThemedUIPalette palette { get { return (target as ThemedImage)?.palette; } }
    SerializedProperty colorIndexProperty;
    SerializedProperty typeProperty;

    void OnEnable()
    {        
        colorIndexProperty = serializedObject.FindProperty("colorIndex");
        typeProperty = serializedObject.FindProperty("m_Type");
    }

    public override void OnInspectorGUI()
    {
        if (serializedObject == null)
            return;
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
        DrawColorButtons();
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

    private bool DrawColorButtons()
    {
        if (!palette)
            return false;
        Texture2D previousButtonSkin = GUI.skin.button.normal.background;
        GUI.skin.button.normal.background = Texture2D.whiteTexture;
        float width = Screen.width - 5;
        int selectedColor = colorIndexProperty.intValue;
        Color[] colors = palette.Colors;

        int columns = Mathf.FloorToInt(width / (cellSize * 3 + 5));
        int rows = Mathf.CeilToInt(colors.Length / (float)columns);
        while (Mathf.CeilToInt(colors.Length / (float)(columns - 1)) == rows)
            columns--;

        for (int y = 0; y < rows; y++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            for (int x = 0; x < columns; x++)
            {
                int i = x + y * columns;
                if (i >= colors.Length)
                    break;
                colors[i].a = 1;

                GUI.backgroundColor = colors[i];
                GUI.contentColor = GetFontColorForBackground(GUI.backgroundColor);

                if (GUILayout.Button($"{ColorToHex(colors[i])}", GUILayout.Width(cellSize * 3), GUILayout.Height(cellSize)))
                {
                    colorIndexProperty.intValue = i;
                    return true;
                }
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        GUI.skin.button.normal.background = previousButtonSkin;
        GUI.backgroundColor = Color.white;
        return false;
    }
    private Color GetFontColorForBackground(Color background) => Mathf.RoundToInt(background.r) + Mathf.RoundToInt(background.g) + Mathf.RoundToInt(background.b) > 1.5f ? Color.black : Color.white;
    private string ColorToHex(Color color) => $"#{Mathf.RoundToInt(color.r * 255):X2}{Mathf.RoundToInt(color.g * 255):X2}{Mathf.RoundToInt(color.b * 255):X2}";
}