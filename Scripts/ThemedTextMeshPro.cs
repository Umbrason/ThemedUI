using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(MeshRenderer))]
public class ThemedTextMeshPro : TextMeshProUGUI
{
    public byte colorIndex;
    public ThemedUIPalette palette;

    public override Material material { get { return palette?.Material_TMPFont ? palette.Material_TMPFont : base.material; } }
    public override Material materialForRendering { get { return palette?.Material_TMPFont ? palette.Material_TMPFont : base.materialForRendering; } }

    protected override void GenerateTextMesh()
    {
        base.GenerateTextMesh();
        Vector2[] uvs = m_mesh.uv;
        for (int i = 0; i < uvs.Length; i++)        
            uvs[i] += Vector2.right * colorIndex;
        m_mesh.uv = uvs;        
        canvasRenderer.SetMesh(m_mesh);
    }

    public static explicit operator TextMeshPro(ThemedTextMeshPro textMeshPro) => (TextMeshPro)textMeshPro;

}
