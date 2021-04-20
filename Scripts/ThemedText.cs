using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemedText : Text
{
    public byte colorIndex;
    public ThemedUIPalette palette;

    public override Material material { get { return palette?.Material_Font ? palette.Material_Font : base.material; } }
    public override Material materialForRendering { get { return palette?.Material_Font ? palette.Material_Font : base.materialForRendering; } }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {        
        base.OnPopulateMesh(toFill);
        Mesh temp = new Mesh();
        toFill.FillMesh(temp);
        for (int i = 0; i < toFill.currentVertCount; i++)
        {
            UIVertex vertex = new UIVertex();
            vertex.position = temp.vertices[i];
            vertex.normal = temp.normals[i];
            vertex.uv0 = temp.uv[i];
            vertex.color = new Color32(colorIndex, colorIndex, colorIndex, colorIndex);
            toFill.SetUIVertex(vertex, i);
        }        
    }
}
