using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (PerlinNoise))]
public class NoiseEditor : Editor
{
    bool autoUpdate=false;
    public override void OnInspectorGUI() {
        
        PerlinNoise noise = (PerlinNoise)target;
       // TerrainGenerator terrain = (TerrainGenerator)target;
        autoUpdate=EditorGUILayout.Toggle("Auto", autoUpdate);
      
       if(DrawDefaultInspector()){
           if(autoUpdate) {
               noise.GenerateTexture();
            
           }
       }
        if(GUILayout.Button ("Generate")) {
            noise.GenerateTexture();
        
            
        }


        
        
        

    }
}   