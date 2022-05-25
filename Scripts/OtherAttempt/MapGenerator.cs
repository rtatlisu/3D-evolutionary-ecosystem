using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColorMap, Mesh};
    public DrawMode drawmode;
   public int mapWidth;
   public int mapHeight;
   public float noiseScale;
   public bool autoUpdate;
   [Range(0,1)]
   public float persistance;
   
   public float lacunarity;
   public int octaves;
   public float meshHeightMultiplier;
   
   public int seed; 
   public Vector2 offset;
   public TerrainType[] regions;


   public void GenerateMap() {
       float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth,mapHeight,seed, noiseScale, octaves, persistance, 
       lacunarity, offset);

        Color[] colorMap = new Color[mapWidth*mapHeight];
       for(int y = 0; y< mapHeight; y++) {
           for(int x = 0; x < mapWidth; x++) {
               float currentHeight = noiseMap[x,y];
               for(int i = 0; i < regions.Length; i++) {
                   if(currentHeight <= regions[i].height) {
                       colorMap[y*mapWidth+x] = regions[i].color;
                       break;
                   }
               }
           }
       }

       MapDisplay display = FindObjectOfType<MapDisplay>();
       if(drawmode==DrawMode.NoiseMap) {
              display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
       }
       else if(drawmode==DrawMode.ColorMap) {
           display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
       }
       else if (drawmode==DrawMode.Mesh) {
           display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier), 
           TextureGenerator.TextureFromColorMap(colorMap, mapWidth,mapHeight));
       }
   }

   void OnValidate() {
       if(mapWidth < 1) {
           mapWidth=1;
       }
       if(mapHeight<1) {
           mapHeight=1;
       }
       if(lacunarity<1){
           lacunarity=1;
       }
       if(octaves<0) {
           octaves=0;
       } 
   }

}
[System.Serializable]
public struct TerrainType {
    public string name;
    public float height;
    public Color color;
}
