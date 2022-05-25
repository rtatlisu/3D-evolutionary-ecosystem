using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mesh2Noise 
{
   public static Texture2D texture;
    public static float[,] noiseMap;
   public static  bool[,] waterTiles;
    static float gen;
    public enum DrawMode {Noise, Color};
   public  static DrawMode drawMode;
   public static TerrainType[] regions;
   public static Color[] colors;
   
  
 
   
public static float[,] Noise(int width, int height, float scale, int octaves, float lacunarity,
float persistency, Vector2 offset, int seed) {

     texture = new Texture2D(75,75);
     noiseMap = new float[texture.width,texture.height];
     waterTiles = new bool[texture.width,texture.height];

     System.Random prng = new System.Random(seed);
     Vector2[] octaveOffsets = new Vector2[octaves];
        
        for(int i = 0; i < octaves; i++) {
            float offsetX = prng.Next(-100000,100000) + offset.x;
            float offsetY = prng.Next(-100000,100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX,offsetY);
        }
         float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;
        float halfWidth = width/2f;
        float halfHeight = height/2f;
if(scale==0) {
  scale = 0.001f;
}
  
     for(int y = 0; y < texture.height; y++) {
       for(int x = 0; x < texture.width; x++) {
            float amplitude = 1;
            float frequency = 1;
            float noiseHeight = 0;

            for(int i = 0; i < octaves; i++) {
                    float xCoord =  (x-halfWidth)/scale * frequency + octaveOffsets[i].x;
                    float yCoord =  (y-halfHeight)/scale * frequency + octaveOffsets[i].y;
                float sample = Mathf.PerlinNoise(xCoord,yCoord)*2-1;
                noiseHeight += sample*amplitude;
                amplitude *= persistency;
                frequency *= lacunarity;
            }

            if(noiseHeight > maxNoiseHeight) {
                 maxNoiseHeight = noiseHeight;
    } 
            else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
            }
            noiseMap[y,x] = noiseHeight;
         
                
        }
           
       }
     for(int y = 0; y < texture.height; y++) {
        for(int x = 0; x < texture.width; x++) {
        noiseMap[y,x] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[y,x]);
         }
     }

     return noiseMap;
   }

   public static Texture2D setTexture() {
       colors = new Color[texture.height*texture.width];

     for(int y = 0; y < texture.height; y++) {
       for(int x = 0; x < texture.width; x++) {
         if(drawMode == DrawMode.Noise) {
           colors[x + y*texture.height] = new Color(noiseMap[y,x],
           noiseMap[y,x],noiseMap[y,x]);
         }
         else if (drawMode == DrawMode.Color) {
           for(int i = 0; i < regions.Length; i++) {
             if(noiseMap[y,x] <= regions[i].height) {
                 if(regions[i].name == "water") {
                     waterTiles[y,x] = true;
                 } 
               colors[x +y*texture.height] = regions[i].color;
               break;
             }
           }
         }
       }
     }
     texture.SetPixels(colors);
    texture.filterMode=FilterMode.Point;
       texture.wrapMode=TextureWrapMode.Clamp;
     texture.Apply();

       return texture;
   }
     
}
