using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;


public static class ThemedUIEditorUtility
{
    public static ThemedUIPalette ActivePalette { get { return activePalette ??= (ThemedUIPalette)AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(EditorPrefs.GetString("ThemedUI.ActivePalette"))); } set { activePalette = value; EditorPrefs.SetString("ThemedUI.ActivePalette", AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(value)).ToString()); } }
    private static ThemedUIPalette activePalette;

    [MenuItem("Assets/Set Default UIPalette")]
    public static void SetActivePalette()
    {
        ActivePalette = Selection.activeObject as ThemedUIPalette ?? ActivePalette;
    }
    [MenuItem("Assets/Set Default UIPalette", true)]
    private static bool EvaluateSetActivePalette()
    {
        return Selection.activeObject is ThemedUIPalette;
    }

    [MenuItem("GameObject/UI/ThemedImage")]
    public static void CreateThemedImage()
    {
        Undo.SetCurrentGroupName("create themed image");
        if (Selection.transforms.Length > 0)
            foreach (var transform in Selection.transforms)
            {
                InstantiateThemedImage(transform);
            }
        else
            InstantiateThemedImage();
        Undo.IncrementCurrentGroup();
    }
    private static void InstantiateThemedImage(Transform parent = null)
    {
        var go = new GameObject("Themed Image", typeof(ThemedImage));
        go.transform.SetParent(parent);
        var component = go.GetComponent<ThemedImage>();
        if (component)
            component.palette = ActivePalette;
        if (parent)
            go.layer = parent.gameObject.layer;
        Undo.RegisterCreatedObjectUndo(go, "Create Themed Image");
    }

    [MenuItem("GameObject/UI/ThemedText")]
    public static void CreateThemedText()
    {
        Undo.SetCurrentGroupName("create themed text");
        if (Selection.transforms.Length > 0)
            foreach (var transform in Selection.transforms)
            {
                InstantiateThemedText(transform);
            }
        else
            InstantiateThemedText();
        Undo.IncrementCurrentGroup();
    }
    private static void InstantiateThemedText(Transform parent = null)
    {
        var go = new GameObject("Themed Text", typeof(ThemedText));
        go.transform.SetParent(parent);
        var component = go.GetComponent<ThemedText>();
        if (component)
            component.palette = ActivePalette;
        if (parent)
            go.layer = parent.gameObject.layer;
        Undo.RegisterCreatedObjectUndo(go, "Create Themed Text");
    }

    [MenuItem("GameObject/UI/ThemedTMPro")]
    public static void CreateThemedTextMeshPro()
    {
        Undo.SetCurrentGroupName("create themed TMPro");
        if (Selection.transforms.Length > 0)
            foreach (var transform in Selection.transforms)
            {
                InstantiateThemedTextMeshPro(transform);
            }
        else
            InstantiateThemedTextMeshPro();
        Undo.IncrementCurrentGroup();
    }
    private static void InstantiateThemedTextMeshPro(Transform parent = null)
    {
        var go = new GameObject("Themed TMPro", typeof(ThemedTextMeshPro));
        go.transform.SetParent(parent);
        var component = go.GetComponent<ThemedTextMeshPro>();
        if (component)
            component.palette = ActivePalette;
        if (parent)
            go.layer = parent.gameObject.layer;
        Undo.RegisterCreatedObjectUndo(go, "Create Themed TMPro");
    }


    [MenuItem("CONTEXT/Image/Convert To ThemedImage", false, 51)]
    public static void MakeImageThemed()
    {
        Undo.SetCurrentGroupName("convert to themed image");
        foreach (GameObject go in Selection.gameObjects)
        {
            Image imageComponent = go.GetComponent<Image>();
            var component = ReplaceComponent<Image, ThemedImage>(imageComponent);
            if (component && component.palette == null)
                component.palette = ActivePalette;
        }
        Undo.IncrementCurrentGroup();
    }

    [MenuItem("CONTEXT/Text/Convert To ThemedText", false, 51)]
    public static void MakeTextThemed()
    {
        Undo.SetCurrentGroupName("convert to themed text");
        foreach (GameObject go in Selection.gameObjects)
        {
            Text textComponent = go.GetComponent<Text>();
            var component = ReplaceComponent<Text, ThemedText>(textComponent);
            if (component && component.palette == null)
                component.palette = ActivePalette;
        }
        Undo.IncrementCurrentGroup();
    }

    [MenuItem("CONTEXT/TextMeshProUGUI/Convert To ThemedTextMeshPro", false, 51)]
    public static void MakeTextMeshProThemed()
    {
        Undo.SetCurrentGroupName("convert to themed tmpro");
        foreach (GameObject go in Selection.gameObjects)
        {
            TextMeshProUGUI textMeshProComponent = go.GetComponent<TextMeshProUGUI>();
            var component = ReplaceComponent<TextMeshProUGUI, ThemedTextMeshPro>(textMeshProComponent);
            if (component && component.palette == null)
                component.palette = ActivePalette;
        }
        Undo.IncrementCurrentGroup();
    }

    private static K ReplaceComponent<T, K>(T original) where T : Component where K : Component
    {
        if (original == null)
            return null;
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy;
        GameObject gameObject = original.gameObject;
        Dictionary<string, object> propertyValues = new Dictionary<string, object>();
        foreach (PropertyInfo info in typeof(T).GetProperties(flags))
            if (info.CanRead)
                propertyValues.Add(info.Name, info.GetValue(original));
        Undo.RecordObject(original, $"replacing component of type {typeof(T)} of object {original.name} with component of type {typeof(K)}");
        Undo.DestroyObjectImmediate(original);
        K newComponent = Undo.AddComponent<K>(gameObject);
        foreach (PropertyInfo info in typeof(T).GetProperties(flags))
            if (info.CanWrite && typeof(K).GetProperty(info.Name) != null)
                info.SetValue(newComponent, propertyValues[info.Name]);
        return newComponent;
    }
    public static bool DrawColorButtons(ThemedUIPalette palette, ref SerializedProperty colorIndexProperty, float cellSize)
    {
        if (!palette)
            return false;
        Texture2D previousButtonSkin = GUI.skin.button.normal.background;
        GUI.skin.button.normal.background = Texture2D.whiteTexture;
        Color oldContentColor = GUI.contentColor;
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
                    GUI.skin.button.normal.background = previousButtonSkin;
                    return true;
                }
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        GUI.skin.button.normal.background = previousButtonSkin;        
        GUI.backgroundColor = Color.white;
        GUI.contentColor = oldContentColor;
        return false;
    }

    private static Color GetFontColorForBackground(Color background) => Mathf.RoundToInt(background.r) + Mathf.RoundToInt(background.g) + Mathf.RoundToInt(background.b) > 1.5f ? Color.black : Color.white;
    private static string ColorToHex(Color color) => $"#{Mathf.RoundToInt(color.r * 255):X2}{Mathf.RoundToInt(color.g * 255):X2}{Mathf.RoundToInt(color.b * 255):X2}";
}
