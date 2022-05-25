using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Mesh2))]
public class Mesh2Editor : Editor
{
    bool autoUpdate=false;
    public override void OnInspectorGUI() {
        
        Mesh2 mesh2 = (Mesh2)target;
       // TerrainGenerator terrain = (TerrainGenerator)target;
        autoUpdate=EditorGUILayout.Toggle("Auto", autoUpdate);
      
       if(DrawDefaultInspector()){
           if(autoUpdate) {
               mesh2.GenerateMesh();   
             Mesh2Noise.setTexture();
               mesh2.GenerateHeightMap();
               mesh2.AddMesh();
            
           }
       }
        if(GUILayout.Button ("Generate")) {
             mesh2.GenerateMesh();           
             Mesh2Noise.setTexture();
            mesh2.GenerateHeightMap();
               mesh2.AddMesh();
        }
        
        
    }
 
}
