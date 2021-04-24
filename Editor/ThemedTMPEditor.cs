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
        DrawColorButtons();
        if (serializedObject.hasModifiedProperties)
            serializedObject.ApplyModifiedProperties();        
        EditorGUILayout.Separator();


        base.OnInspectorGUI();

    }

    private void DrawColorButtons()
    {
        if (!palette)
            return;
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
                }
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        GUI.skin.button.normal.background = previousButtonSkin;
        GUI.backgroundColor = Color.white;
        GUI.contentColor = Color.white;
    }
    private Color GetFontColorForBackground(Color background) => Mathf.RoundToInt(background.r) + Mathf.RoundToInt(background.g) + Mathf.RoundToInt(background.b) > 1.5f ? Color.black : Color.white;
    private string ColorToHex(Color color) => $"#{Mathf.RoundToInt(color.r * 255):X2}{Mathf.RoundToInt(color.g * 255):X2}{Mathf.RoundToInt(color.b * 255):X2}";
}
