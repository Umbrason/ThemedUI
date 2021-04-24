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
        GameObject ImageGO = Selection.activeGameObject;
        Image textComponent = ImageGO.GetComponent<Image>();
        ReplaceComponent<Image, ThemedImage>(textComponent);
    }
    [MenuItem("CONTEXT/Text/Convert To ThemedText", false, 51)]
    public static void MakeTextThemed()
    {
        GameObject TextGO = Selection.activeGameObject;
        Text textComponent = TextGO.GetComponent<Text>();
        ReplaceComponent<Text, ThemedText>(textComponent);
    }

    [MenuItem("CONTEXT/TextMeshProUGUI/Convert To ThemedTextMeshPro", false, 51)]
    public static void MakeTextMeshProThemed()
    {
        GameObject TextGO = Selection.activeGameObject;
        TextMeshProUGUI textComponent = TextGO.GetComponent<TextMeshProUGUI>();
        ReplaceComponent<TextMeshProUGUI, ThemedTextMeshPro>(textComponent);
    }

    
    //Revert code not ready
    /*[MenuItem("CONTEXT/ThemedImage/Revert to Image", false, 51)]
    public static void RevertImageThemed()
    {
        GameObject ImageGO = Selection.activeGameObject;
        ThemedImage textComponent = ImageGO.GetComponent<ThemedImage>();
        ReplaceComponent<ThemedImage, Image>(textComponent);
    }
    [MenuItem("CONTEXT/ThemedText/Revert To Text", false, 51)]
    public static void RevertTextThemed()
    {
        GameObject TextGO = Selection.activeGameObject;
        ThemedText textComponent = TextGO.GetComponent<ThemedText>();
        ReplaceComponent<ThemedText, Text>(textComponent);
    }

    [MenuItem("CONTEXT/ThemedTextMeshPro/Revert To TextMeshProUGUI", false, 51)]
    public static void RevertTextMeshProThemed()
    {
        GameObject TextGO = Selection.activeGameObject;
        ThemedTextMeshPro textComponent = TextGO.GetComponent<ThemedTextMeshPro>();
        ReplaceComponent<ThemedTextMeshPro, TextMeshProUGUI>(textComponent);
    }*/

    private static void ReplaceComponent<T, K>(T original) where T : Component where K : Component
    {
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
