using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadScript : MonoBehaviour
{
    Texture2D texture;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    // Start is called before the first frame update
    void Start()
    {
        texture = new Texture2D(10,10);
        texture.filterMode=FilterMode.Point;
        texture.wrapMode=TextureWrapMode.Clamp;

        for(int x = 0; x < texture.width; x++) {
            for(int y = 0; y < texture.height; y++) {
                texture.SetPixel(x,y,GenerateColor(x,y));
            }
        }
        texture.Apply();
        GetComponent<Renderer>().sharedMaterial.mainTexture=texture;

        vertices = new Vector3[]{
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0),
            new Vector3(1,0,1)
        };

        triangles = new int[] {
            0,1,2,
            2,1,3
        };

        uvs = new Vector2[] {
           new Vector2(0,0),
           new Vector2(0,1),
           new Vector2(1,0),
           new Vector2(1,1)
        };

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices=vertices;
        mesh.triangles=triangles;
        mesh.uv=uvs;
        mesh.RecalculateBounds();
    }

    Color GenerateColor(int x, int y) {
        float sample = Mathf.PerlinNoise(x/(float)texture.width, y/(float)texture.height);

        return new Color(sample,sample,sample);
    }

  
}
