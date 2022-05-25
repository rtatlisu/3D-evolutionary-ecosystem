using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundGeneration : MonoBehaviour
{
   Mesh mesh;
   MeshCollider meshCollider;
   int[] triangles;
   Vector3[] vertices;
   public int xSize=20;
   public int zSize=20;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
       GetComponent<MeshFilter>().mesh = mesh;
      

      CreateShape();

      MeshUpdate();

       
    }

    


     void CreateShape() {
         vertices = new Vector3[(xSize+1)*(zSize+1)];

         for(int i = 0,z = 0; z <= zSize; z++ ) {
             for(int x = 0; x <= xSize; x++) {
                 vertices[i] = new Vector3(x, 0, z);
                 i++;
             }
         }  

        triangles = new int[(xSize*zSize)*3*2];
        for(int z=0,i=0; z<zSize; z++) {
            for(int x=0; x<xSize; x++) {
                triangles[i]=x+(z*(xSize+1));
                triangles[i+1] = xSize+(x+1)+(z*(xSize+1));
                triangles[i+2] = x+1+(z*(xSize+1));

                triangles[i+3]= xSize+(x+1)+(z*(xSize+1));
                triangles[i+4] = xSize+(x+2)+(z*(xSize+1));
                triangles[i+5] = x+1+(z*(xSize+1));
                i+=6;
              
            }
        }


    }

  /*  public void OnDrawGizmos() {

        if(vertices==null) {
            return;
        }
        for(int i = 0; i < vertices.Length; i++) {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
*/
    void MeshUpdate() {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

 
}
