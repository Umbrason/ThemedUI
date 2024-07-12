using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Themed UI Palette", fileName = "new Palette")]
public class ThemedUIPalette : ScriptableObject
{
    [SerializeField]
    private List<Color> colors = new List<Color>();
    public Color[] Colors { get { return colors.ToArray(); } }
    public int ColorCount { get { return colors.Count; } }
    [SerializeField]
    private Material material;
    public Material Material { get { return material; } }

    [SerializeField]
    private Material material_Font;
    public Material Material_Font { get { return material_Font; } }

    [SerializeField]
    private Material material_TMPFont;
    public Material Material_TMPFont { get { return material_TMPFont; } }

    [SerializeField]
    private Texture2D texture;
    public Texture2D Texture => texture;

#if UNITY_EDITOR
    public void InitializeMaterial_Font()
    {
        if (material_Font)
            return;
        material_Font = new Material(Shader.Find("ThemedUI/Font"));
        AssetDatabase.AddObjectToAsset(material_Font, this);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this));
    }
    public void InitializeMaterial_TMPFont()
    {
        if (material_TMPFont)
            return;
        material_TMPFont = new Material(Shader.Find("ThemedUI/TMP"));
        AssetDatabase.AddObjectToAsset(material_TMPFont, this);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this));
    }
    public void InitializeMaterial()
    {
        if (material)
            return;
        material = new Material(Shader.Find("ThemedUI/Image"));
        AssetDatabase.AddObjectToAsset(material, this);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this));
    }
    public void InitializeTexture()
    {
        if (texture)
            return;
        texture = new Texture2D(Mathf.Max(colors.Count, 1), 1, TextureFormat.ARGB32, false);
        texture.filterMode = FilterMode.Point;
        AssetDatabase.AddObjectToAsset(texture, this);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(this));
    }

    public void UpdateMaterial()
    {
        if (!material)
            return;
        material.name = $"{this.name}_material";
        material.SetTexture("_Palette", texture);
        material.SetFloat("_ColorCount", colors.Count);
    }

    public void UpdateMaterial_TMPFont()
    {
        if (!material_TMPFont)
            return;
        material_TMPFont.name = $"{this.name}_material";
        material_TMPFont.SetTexture("_Palette", texture);
        material_TMPFont.SetFloat("_ColorCount", colors.Count);
    }

    public void UpdateMaterial_Font()
    {
        if (!material_Font)
            return;
        material_Font.name = $"{this.name}_material_font";
        material_Font.SetTexture("_Palette", texture);
        material_Font.SetFloat("_ColorCount", colors.Count);
    }

    public void UpdateTexture()
    {
        if (!texture)
            return;
        if (!(texture.width == colors.Count))
        {
            DestroyImmediate(texture, true);
            texture = null;
            InitializeTexture();
        }
        texture.name = $"{this.name}_texture";
        texture.SetPixels(0, 0, colors.Count, 1, colors.ToArray());
        texture.Apply();
    }

    
#endif
}