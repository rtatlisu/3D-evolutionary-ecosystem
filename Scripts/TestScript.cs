using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestScript : MonoBehaviour
{
   int[] triangles;
   Vector3[] vertices;
   Vector2[] uvs;
   Color[] colors;
   public bool autoUpdate=false;
   int triangleIndex;
   bool needsUpdate=false;
   Texture2D texture;
   Material[] mat = new Material[2];
   Renderer rend;
   int[][] triDex = new int[2][];
  

void Start() {
    rend = GetComponent<Renderer>();
    mat[0] = new Material(Shader.Find("Unlit/Texture"));
    mat[1] = new Material(Shader.Find("Unlit/Texture"));
    mat[0].mainTexture = Resources.Load ("colorTest") as Texture2D;
    mat[1].mainTexture = Resources.Load ("MyTest") as Texture2D;
    rend.materials=mat;

  texture = new Texture2D(10,10);
  Color color; 
  for(int i = 0; i < texture.width; i++) {
      for(int j = 0; j < texture.height; j++) {
          float sample = Mathf.PerlinNoise(i/(float)texture.width,j/(float)texture.height);
              color= new Color(sample,sample,sample);
              texture.SetPixel(i,j,color);
      }
  }
  texture.Apply();
    GetComponent<Renderer>().sharedMaterial.mainTexture = texture;

    CreateMesh();
    UpdateMesh();
}
   


    
 



void CreateMesh() {
     int width=2;
     int breadth=2;
     int height=2;
  //  vertices = new Vector3[width*breadth];
   // triangles = new int[(width-1)*(breadth-1)*6];
   vertices = new Vector3[width*breadth*2];
   triangles = new int[(width-1)*(breadth-1)*12*3];
    //2 -> 18
    //3 ->36
   
    int vertexIndex=0;
    triangleIndex=0;
    
     for(int y = 0; y < height; y++) {
         if(y==1) {vertexIndex=0;}
        for(int z = 0; z < breadth; z++) {
            for(int x = 0; x < width; x++) {
                vertices[z*breadth + x + y*width*breadth] = new Vector3(x,y,z);
                
                if(x < width-1 && z < breadth-1) {
                   
                    //bottom
                    if(y!= 1) {
                        AddTriangle(vertexIndex+1, vertexIndex+width, vertexIndex); 
                       // Debug.Log((vertexIndex+1)+" "+ (vertexIndex+width)+" "+ vertexIndex);
                        AddTriangle(vertexIndex+1, vertexIndex+width+1, vertexIndex+width);
                }
                //right
                if(y==1) {
                    
                    AddTriangle(vertexIndex+1, vertexIndex+width*breadth+1, vertexIndex+width+1);
                    Debug.Log((vertexIndex+1) +" "+(vertexIndex+width*breadth+1)+" "+ (vertexIndex+width+1));
                    AddTriangle(vertexIndex+width*breadth+1, vertexIndex+width*breadth+width+1,
                     vertexIndex+width+1);
                
                //back
                AddTriangle(vertexIndex+width*breadth+width+1, vertexIndex+width, 
                vertexIndex+width+1);
                AddTriangle(vertexIndex+width*breadth+width+1, vertexIndex+width*breadth+width,
                     vertexIndex+width);
                
                //left
                AddTriangle(vertexIndex+width, vertexIndex+width*breadth+width, 
                vertexIndex);
                AddTriangle(vertexIndex+width*breadth+width, vertexIndex+breadth*width,
                     vertexIndex);
                
               //front
                AddTriangle(vertexIndex, vertexIndex+width*breadth+1, 
                vertexIndex+1);
                AddTriangle(vertexIndex, vertexIndex+breadth*width,
                     vertexIndex+breadth*width+1);
                
                //top
                AddTriangle(vertexIndex+width*breadth, vertexIndex+width*breadth+width, 
                vertexIndex+width*breadth+1+width);
                AddTriangle(vertexIndex+width*breadth, vertexIndex+breadth*width+1+width,
                     vertexIndex+breadth*width+1);
                }

                }
                vertexIndex++;
            }
        }
        
}

   //0,0,0
   //1,0,0
   //0,0,1
   //1,0,1

   //0,1,0
   //1,1,0
   //0,1,1
   //1,1,1


    
   
  
}

void AddTriangle(int a, int b, int c) {
    triangles[triangleIndex] = a;
    triangles[triangleIndex+1] = b;
    triangles[triangleIndex+2] = c;
    triangleIndex+=3;
}

void UpdateMesh() {
    Mesh mesh = new Mesh();
    GetComponent<MeshFilter>().mesh=mesh;
    uvs = new Vector2[vertices.Length];
   for(int i = 0; i < vertices.Length; i++) {
       uvs[i] = new Vector2(vertices[i].x,vertices[i].y);
       
   }
  
  mesh.subMeshCount=2;
  
    mesh.Clear();
    
    mesh.vertices=vertices;
    mesh.triangles = triangles;
    mesh.uv = uvs;
  
  triDex[0] = new int[6] {0,1,2,2,1,3};
  mesh.SetTriangles(triDex[0],0);
    mesh.RecalculateNormals();
    Debug.Log(mesh.subMeshCount);
}



   
}
