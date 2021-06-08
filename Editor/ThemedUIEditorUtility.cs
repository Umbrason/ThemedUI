using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;


public static class ThemedUIEditorUtility
{
    [MenuItem("GameObject/UI/ThemedImage")]
    public static void CreateThemedImage()
    {
        Undo.SetCurrentGroupName("create themed image");
        foreach (GameObject go in Selection.gameObjects)
        {

            GameObject child = new GameObject("Themed Image", typeof(ThemedImage));
            child.transform.SetParent(go.transform);
            child.layer = go.layer;
            Undo.RegisterCreatedObjectUndo(child,"Create Themed Image");
        }
        Undo.IncrementCurrentGroup();
    }

    [MenuItem("GameObject/UI/ThemedText")]
    public static void CreateThemedText()
    {
        Undo.SetCurrentGroupName("create themed text");
        foreach (GameObject go in Selection.gameObjects)
        {

            GameObject child = new GameObject("Themed Text", typeof(ThemedText));
            child.transform.SetParent(go.transform);
            child.layer = go.layer;
            Undo.RegisterCreatedObjectUndo(child,"Create Themed Text");
        }
        Undo.IncrementCurrentGroup();
    }

    [MenuItem("GameObject/UI/ThemedTMPro")]
    public static void CreateThemedTextMeshPro()
    {
        Undo.SetCurrentGroupName("create themed TMPro");
        foreach (GameObject go in Selection.gameObjects)
        {
            GameObject child = new GameObject("Themed TMPro", typeof(ThemedTextMeshPro));
            child.transform.SetParent(go.transform);
            child.layer = go.layer;
            Undo.RegisterCreatedObjectUndo(child,"Create Themed TMPro");
        }
        Undo.IncrementCurrentGroup();
    }


    [MenuItem("CONTEXT/Image/Convert To ThemedImage", false, 51)]
    public static void MakeImageThemed()
    {
        Undo.SetCurrentGroupName("convert to themed image");
        foreach (GameObject go in Selection.gameObjects)
        {
            Image textComponent = go.GetComponent<Image>();
            ReplaceComponent<Image, ThemedImage>(textComponent);
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
            ReplaceComponent<Text, ThemedText>(textComponent);
        }
        Undo.IncrementCurrentGroup();
    }

    [MenuItem("CONTEXT/TextMeshProUGUI/Convert To ThemedTextMeshPro", false, 51)]
    public static void MakeTextMeshProThemed()
    {
        Undo.SetCurrentGroupName("convert to themed tmpro");
        foreach (GameObject go in Selection.gameObjects)
        {
            TextMeshProUGUI textComponent = go.GetComponent<TextMeshProUGUI>();
            ReplaceComponent<TextMeshProUGUI, ThemedTextMeshPro>(textComponent);
        }
        Undo.IncrementCurrentGroup();
    }

    private static void ReplaceComponent<T, K>(T original) where T : Component where K : Component
    {
        if (original == null)
            return;
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
    }
}
