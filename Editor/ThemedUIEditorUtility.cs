using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;


public static class ThemedUIEditorUtility
{
    [MenuItem("CONTEXT/Image/Convert To ThemedImage", false, 51)]
    public static void MakeImageThemed()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            Image textComponent = go.GetComponent<Image>();
            ReplaceComponent<Image, ThemedImage>(textComponent);
        }
    }
    [MenuItem("CONTEXT/Text/Convert To ThemedText", false, 51)]
    public static void MakeTextThemed()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            Text textComponent = go.GetComponent<Text>();
            ReplaceComponent<Text, ThemedText>(textComponent);
        }
    }

    [MenuItem("CONTEXT/TextMeshProUGUI/Convert To ThemedTextMeshPro", false, 51)]
    public static void MakeTextMeshProThemed()
    {        
        foreach (GameObject go in Selection.gameObjects)
        {
            TextMeshProUGUI textComponent = go.GetComponent<TextMeshProUGUI>();
            ReplaceComponent<TextMeshProUGUI, ThemedTextMeshPro>(textComponent);
        }
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
