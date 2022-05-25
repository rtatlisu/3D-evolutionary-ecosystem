using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColorMap};
    public DrawMode drawmode;
    public int width = 100;
    public int height = 100;
    public float scale = 20f;
  // public float offsetX = 100f;
   // public float offsetY = 100f;
    public int octaves = 1;
    public float lacunarity = 1;
    public float persistency = 1;
    public Vector2 offset;
    public int seed;
    public TerrainType[] regions;
    public float[,] noiseMap ;
    public Renderer textureRenderer;
    public Texture2D texture;
    
   
    
 
       // Renderer renderer = GetComponent<Renderer>();
       // renderer.material.mainTexture = GenerateTexture();
    

    public void  GenerateTexture() {
       
         texture = new Texture2D(width,height);
        noiseMap = new float[width,height];
        Color[] colorMap = new Color[width*height];
         

        CalculateColor();
            
        
       if(drawmode==DrawMode.NoiseMap) { 
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
               colorMap[y*width+x] = Color.Lerp(Color.black, Color.white,
               noiseMap[x,y]);

            }
        }
        texture.SetPixels(colorMap);
       }

       if(drawmode==DrawMode.ColorMap) {
           for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
             float currentHeight = noiseMap[x,y];
             for(int i = 0; i < regions.Length; i++) {
                 if(currentHeight <= regions[i].height) {
                     colorMap[y*width+x] = regions[i].color;
                     break;
                 }
             }
            }
        }
        texture.SetPixels(colorMap);
       }
     /*  Mesh2 mesh2;
       mesh2 = gameObject.AddComponent<Mesh2>();
       mesh2.Start();
       mesh2.GenerateMesh(noiseMap);
   */
  
       texture.filterMode=FilterMode.Point;
       texture.wrapMode=TextureWrapMode.Clamp;
        texture.Apply();
        textureRenderer.sharedMaterial.mainTexture=texture;
      
    }

    float[,] CalculateColor() {

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        
        for(int i = 0; i < octaves; i++) {
            float offsetX = prng.Next(-100000,100000) + offset.x;
            float offsetY = prng.Next(-100000,100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX,offsetY);
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

for(int x = 0; x < width; x++){
    for(int y = 0; y < height; y++) {
        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;

        for(int i = 0; i < octaves; i++) {
                float xCoord = (float) x/scale * frequency + octaveOffsets[i].x;
                float yCoord = (float) y/scale * frequency + octaveOffsets[i].y;
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
    noiseMap[x,y] = noiseHeight;
    }
}

for(int y = 0; y < height; y++) {
    for(int x = 0; x < width; x++) {
        noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x,y]);
    }
}
            
          //  return new Color(sample, sample, sample);
          return noiseMap;
    }
   
}
