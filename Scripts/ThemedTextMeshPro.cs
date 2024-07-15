using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThemedTextMeshPro : TextMeshProUGUI
{
    public byte colorIndex;
    public ThemedUIPalette palette;

    /* public override Material material { get { return palette?.Material_TMPFont ? palette.Material_TMPFont : base.material; } } */
    public override Material materialForRendering { get { return ModifyMaterial(base.materialForRendering); } }

    Material ModifyMaterial(Material baseMat)
    {
        if (palette != null)
        {
            baseMat = new(baseMat);
            baseMat.shader = palette.Material_TMPFont.shader;
            baseMat.SetTexture("_Palette", palette.Texture);
            baseMat.SetInt("_ColorCount", palette.ColorCount);
        }
        return baseMat;
    }

    protected override void GenerateTextMesh()
    {
        base.GenerateTextMesh();
        Vector2[] uvs = new Vector2[m_mesh.uv.Length];
        for (int i = 0; i < uvs.Length; i++)
            uvs[i] = Vector2.right * (colorIndex);
        m_mesh.uv3 = uvs;
        canvasRenderer.SetMesh(m_mesh);
        var canvas = GetComponentInParent<Canvas>();
        canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord2;
    }

    public static explicit operator TextMeshPro(ThemedTextMeshPro textMeshPro) => (TextMeshPro)textMeshPro;

}
