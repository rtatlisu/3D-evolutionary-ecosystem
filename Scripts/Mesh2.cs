using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Mesh2 : MonoBehaviour
{
  List<Vector3> vertices;
   List<int> triangles;
   List<Vector2> uvs;
    MeshRenderer meshRenderer;
   int verticesIndex;
   public enum DrawMode {Noise, Color};
   public  DrawMode drawMode;
   public float scale;
   public int octaves;
    public float lacunarity;
    public float persistency;
    public Vector2 offset;
    public int seed;
   public TerrainType[] regions;
   float[,] noiseMap;
   int width;
   int height;
    bool[,] waterTilesPerMesh;
    bool[,] landTilesPerMesh;
    int waterRowLength;
    int landRowLength;



   

 
  public void Start() {
      GenerateMesh();   
      Mesh2Noise.setTexture();
      GenerateHeightMap();
      AddMesh();

   }


   public void GenerateMesh() {
    width=75;
    height=75;

     meshRenderer = GetComponent<MeshRenderer>();
     Mesh2Noise.drawMode = (Mesh2Noise.DrawMode)drawMode;
     Mesh2Noise.regions = regions;
     Mesh2Noise.Noise(width, height, scale, octaves, lacunarity, persistency, offset, seed);
     meshRenderer.sharedMaterial.mainTexture = Mesh2Noise.texture; 
 
    
       

     triangles = new List<int>();
     vertices = new List<Vector3>();
     uvs = new List<Vector2>();
      

       verticesIndex=0;
      
    
   
      for(int y = 0; y < height; y++) {
        for(int x = 0; x < width; x++) {
         vertices.Add(new Vector3(x,0,y));
         uvs.Add(new Vector2(x/(float)width, y/(float)height));
/*
        if(vertices.Count >= 638) {
              print("ALARM: " + " vertex: "+ vertices[636]+ " x: " + x + " y: " + y + " xRatio: " + (x/(float)width) + " yRatio: "+
              (y/(float)height));
               print("ALARM: " + " vertex: "+ vertices[637]+ " x: " + x + " y: " + y + " xRatio: " + (x/(float)width) + " yRatio: "+
              (y/(float)height));
        }
   */      
    

          if(x < width-1 && y < height-1) {
          
          //top
            AddTriangle(verticesIndex,verticesIndex+width,verticesIndex+width+1);
            AddTriangle(verticesIndex+width+1,verticesIndex+1,verticesIndex);
          //AddTriangle(verticesIndex,verticesIndex+width+1,verticesIndex+width);
           // AddTriangle(verticesIndex+width+1,verticesIndex,verticesIndex+1);          
          }
          verticesIndex++;
        }
      }
   }

   void AddTriangle(int a, int b, int c) {
     triangles.Add(a);
     triangles.Add(b);
     triangles.Add(c);
   }


   public void GenerateHeightMap() {

  
  //WATER
   waterTilesPerMesh = new bool [height,width];
  for(int y = 0, y2 = 0; y < height; y++) {
    if(y%(width/Mesh2Noise.texture.width)==0 && y!=0) {
      y2++;
    }
    int x2=0;
    for(int x = 0; x < width-((width/Mesh2Noise.texture.width)-1); x=x+(width/Mesh2Noise.texture.width)) {
     for(int i = 0; i < (width/Mesh2Noise.texture.width); i++) {
       waterTilesPerMesh[y,x+i] = Mesh2Noise.waterTiles[y2,x2];
     }
     
      if((x+((width/Mesh2Noise.texture.width)-1)) == (width-1)) {
        waterTilesPerMesh[y,(x+((width/Mesh2Noise.texture.width)-1))] = false;
      }
      x2++;
    }
  }

    for(int y = 0; y < height; y++) {
      int xDiff=0;

       WaterRowLength(y);
      
       for(int x = 0; x < width; x++) {
         if(waterTilesPerMesh[y,x]) {
           vertices.Add(new Vector3(x,0.5f,y));
              uvs.Add(new Vector2(x/(float)width, y/(float)height));
              
            if(x < width-1 && y < height-1) {
   

/*
                //front
                if( y > 0 && !waterTilesPerMesh[y-1,x] || y==0 ) {
                  AddTriangle(x+(y*width), verticesIndex, x+(y*width)+1);
                  AddTriangle(x+(y*width)+1 ,verticesIndex, verticesIndex+1);     
                }

                //right
               if(!waterTilesPerMesh[y,x+1]) {
                 AddTriangle(x+(y*width)+1,verticesIndex+1,x+(y*width)+width+1);
                  AddTriangle(x+(y*width)+width+1,verticesIndex+1,verticesIndex+1+
                  (waterRowLength) +xDiff );
               }
               */
               /*
              print((x+(y*width)+width+1)+" "+(verticesIndex+1)+" "+ (verticesIndex+1+
                  (waterRowLength) +xDiff) + " waterLength: "+(waterRowLength) + " xDif: "+xDiff 
                  +" pseudoLength: "+pseudoWaterRow + " pseudo: "+addPseudo+ " y: "+y);
*/
/*
                //back
                if(!waterTilesPerMesh[y+1,x] || y == height-2) {
                  AddTriangle(verticesIndex+1+waterRowLength+xDiff, x+(y*width)+width,
                  x+(y*width)+width+1);
                  AddTriangle(x+(y*width)+width,verticesIndex+1+waterRowLength+xDiff,
                  verticesIndex+waterRowLength+xDiff);
                }

                //left
                if(x > 0 && !waterTilesPerMesh[y,x-1] || x==0) {
                  AddTriangle(verticesIndex,x+(y*width)+width,verticesIndex+waterRowLength+xDiff);
                  AddTriangle(x+(y*width),x+(y*width)+width,verticesIndex);     
                }
                //top
                AddTriangle(verticesIndex,verticesIndex+waterRowLength+xDiff,
                verticesIndex+1);
                AddTriangle(verticesIndex+1,verticesIndex+waterRowLength+xDiff,
                verticesIndex+1+waterRowLength+xDiff);
*/
           /* 
            print((verticesIndex)+" "+(verticesIndex+waterRowLength+xDiff)
                   +" "+( verticesIndex+1) + " waterLength: "+(waterRowLength) + " xDif: "+xDiff 
                  +" pseudoLength: "+pseudoWaterRow + " pseudo: "+addPseudo+ " y: "+y);
          */
                  
              }
              
              verticesIndex++;
         }


         else if(!waterTilesPerMesh[y,x]) {
           
           if(x > 0 && waterTilesPerMesh[y,x-1]) {
                 vertices.Add(new Vector3(x,0.5f,y));
                 uvs.Add(new Vector2((x-1)/(float)width, y/(float)height));
                 verticesIndex++;
           }

           else if( y > 0 && waterTilesPerMesh[y-1,x]) {
                 vertices.Add(new Vector3(x,0.5f,y));
                 uvs.Add(new Vector2(x/(float)width, (y-1)/(float)height));
                 verticesIndex++;
           }

           else if(x > 0 && y > 0 && waterTilesPerMesh[y-1,x-1]) {
                 vertices.Add(new Vector3(x,0.5f,y));
                 uvs.Add(new Vector2((x-1)/(float)width, (y-1)/(float)height));
                 verticesIndex++;
           }

          //1
           if(y > 0 && x > 0 && y < height-1 && waterTilesPerMesh[y-1,x] && !waterTilesPerMesh[y,x-1] &&
               !waterTilesPerMesh[y+1,x]  && !waterTilesPerMesh[y+1,x-1]
               || x == 0 && y > 0  && waterTilesPerMesh[y-1,x]) {
             xDiff--;
          //   print("1");
           }
           
           //2
           else if(x > 0 && y > 0 && !waterTilesPerMesh[y,x-1] && !waterTilesPerMesh[y-1,x] &&
                 waterTilesPerMesh[y-1,x-1] && y < height-1 && !waterTilesPerMesh[y+1,x-1] &&
                !waterTilesPerMesh[y+1,x]) {
             xDiff--;
           //  print("2");
           }
           
           //3
           else if(x > 0 && y < height-1 && waterTilesPerMesh[y+1,x] && !waterTilesPerMesh[y,x-1] &&
                  y > 0 && !waterTilesPerMesh[y-1,x] && !waterTilesPerMesh[y-1,x-1]
                  || x == 0 && y < height-1 && waterTilesPerMesh[y+1,x]
                  || y == 0 && y < height-1 && x > 0 && waterTilesPerMesh[y+1,x] && !waterTilesPerMesh[y,x-1]) {
             xDiff++;
          //   print("3");
           }
    
          //4
           else if(x > 0 && y < height-1 && !waterTilesPerMesh[y+1,x] && !waterTilesPerMesh[y,x-1] && 
                waterTilesPerMesh[y+1,x-1] && y > 0 && !waterTilesPerMesh[y-1,x-1] && !waterTilesPerMesh[y-1,x]
                || y==0 && y < height-1 && x > 0 && !waterTilesPerMesh[y+1,x] && !waterTilesPerMesh[y,x-1] 
                && waterTilesPerMesh[y+1,x-1]) {
             xDiff++;
          //   print("4");
           }
         }
       }
      }





//LAND

      //convert watertiles into landtiles
      landTilesPerMesh = new bool[width,height];
      for(int y = 0; y < height; y++) {
        for(int x = 0; x < width; x++) {
          if(waterTilesPerMesh[y,x]) {
            landTilesPerMesh[y,x] = false;
          }
          else {
            landTilesPerMesh[y,x] = true;
          }
        }
      }

      //set 99th vertex per row to false,because we have no texture there
      for(int y = 0; y < height; y++) {
          landTilesPerMesh[y,height-1] = false;
      }
      for(int x = 0; x < width; x++) {
          landTilesPerMesh[width-1,x] = false;
      }

      for(int y = 0; y < height; y++) {
      int xDiff=0;
       
       LandRowLength(y);
       
     
       for(int x = 0; x < width; x++) {
       
         if(landTilesPerMesh[y,x]) {
  
         //  print(landRowLength + " + " + xDiff +" = "+(landRowLength+xDiff));
              vertices.Add(new Vector3(x,1f,y));
             uvs.Add(new Vector2(x/(float)width, y/(float)height));
             /*
        if(vertices.Count >= 7466) {
              print("ALARM: " + " vertex: "+ vertices[7464]+ " x: " + x + " y: " + y + " xRatio: " + (x/(float)width) + " yRatio: "+
              (y/(float)height));
               print("ALARM: " + " vertex: "+ vertices[7465]+ " x: " + x + " y: " + y + " xRatio: " + (x/(float)width) + " yRatio: "+
              (y/(float)height));
        }
            */

            if(x < width-1 && y < height-1) {
            
              
          
 
                //front
                if(y > 0 && !landTilesPerMesh[y-1,x] || y==0 ) {
                   AddTriangle(x+(y*width), verticesIndex, x+(y*width)+1);
                   AddTriangle(x+(y*width)+1 ,verticesIndex, verticesIndex+1);
               }
        
                //right
               if(!landTilesPerMesh[y,x+1]) {
                  AddTriangle(x+(y*width)+1,verticesIndex+1,x+(y*width)+width+1);
                  AddTriangle(x+(y*width)+width+1,verticesIndex+1,verticesIndex+1+
                  (landRowLength) +xDiff ); 
               }
               
             /*     
                 print((x+(y*width)+width+1)+ " "+(verticesIndex+1)+ " "+
                  (verticesIndex+1+(landRowLength) +xDiff) +" length: "+landRowLength
                  +" xDiff: " + xDiff+" addPseudo: "+addPseudo +" y: "+y );
             */
               
 
                //back
               if(y < height-1 && !landTilesPerMesh[y+1,x]) {
                  AddTriangle(x+(y*width)+width+1,verticesIndex+1+landRowLength+xDiff,
                  x+(y*width)+width);
                  AddTriangle(x+(y*width)+width,verticesIndex+1+landRowLength+xDiff,
                  verticesIndex+landRowLength+xDiff);
                 
                }
                
                //left
                if(x > 0 && !landTilesPerMesh[y,x-1] || x==0) {
                  AddTriangle(verticesIndex,x+(y*width),x+(y*width)+width);
                  AddTriangle(x+(y*width)+width,verticesIndex+landRowLength+xDiff
                  ,verticesIndex);
                  
                }
                
                //top
                AddTriangle(verticesIndex,verticesIndex+landRowLength+xDiff,
                verticesIndex+1);
                AddTriangle(verticesIndex+1,verticesIndex+landRowLength+xDiff,
                verticesIndex+1+landRowLength+xDiff);
        
     /*
                  print((verticesIndex)+" "+(verticesIndex+landRowLength+xDiff)+
                  " "+(verticesIndex+1) +" length: "+landRowLength
                  +" xDiff: " + xDiff+" addPseudo: "+addPseudo +" y: "+y );

          */
              
              }
              
              verticesIndex++;   
         }


         else if(!landTilesPerMesh[y,x]) {
           
           if(x > 0 && landTilesPerMesh[y,x-1]) {
                 vertices.Add(new Vector3(x,1f,y));
                 uvs.Add(new Vector2((x-1)/(float)width, y/(float)height));
                 verticesIndex++;
                
           }

           else if( y > 0 && landTilesPerMesh[y-1,x]) {
                 vertices.Add(new Vector3(x,1f,y));
                 uvs.Add(new Vector2(x/(float)width, (y-1)/(float)height));
                 verticesIndex++;            
           }

           else if(x > 0 && y > 0 && landTilesPerMesh[y-1,x-1]) {
                 vertices.Add(new Vector3(x,1f,y));
                 uvs.Add(new Vector2((x-1)/(float)width, (y-1)/(float)height));
                 verticesIndex++;          
           }


          //1
           if(y > 0 && y < height-1 && x < width-1 && landTilesPerMesh[y-1,x] && x > 0 
              && !landTilesPerMesh[y+1,x] && x > 0 && !landTilesPerMesh[y,x-1]
              && !landTilesPerMesh[y+1,x-1]
              || x==0 && y > 0 && y < height-1 && landTilesPerMesh[y-1,x] && !landTilesPerMesh[y+1,x] ) {
             xDiff--;
           //  print("1");
           }

          //2
          else if(y < width-1 && landTilesPerMesh[y+1,x] && x > 0 && !landTilesPerMesh[y,x-1]
               && y > 0 && !landTilesPerMesh[y-1,x] && !landTilesPerMesh[y-1,x-1]
               || x==0 && y < height-1 && y > 0 && landTilesPerMesh[y+1,x] && !landTilesPerMesh[y-1,x]
               || y==0 && y < height-1 && x > 0 && landTilesPerMesh[y+1,x] && !landTilesPerMesh[y,x-1]) {
             xDiff++;
          //   print("2");
           }

           //3
           else if( x > 0 &&  y > 0 && landTilesPerMesh[y-1,x-1] && !landTilesPerMesh[y-1,x] &&
                 !landTilesPerMesh[y,x-1] && x < width-1 && y < height-1 && !landTilesPerMesh[y+1,x-1]
                 && !landTilesPerMesh[y+1,x]) {
             xDiff--;
           //  print("3");
           }

           //4
           else if(x > 0 && x < width-1 && y > 0 && y < height-1 && landTilesPerMesh[y+1,x-1] && 
                 !landTilesPerMesh[y+1,x] && !landTilesPerMesh[y,x-1] && !landTilesPerMesh[y-1,x] &&
                !landTilesPerMesh[y-1,x-1]

                || y==0 && x > 0 && y < height-1 && !landTilesPerMesh[y,x-1] && !landTilesPerMesh[y+1,x] && 
                landTilesPerMesh[y+1,x-1] ) {
             xDiff++;
           //  print("4");
           }
         }    
       }
      }   
   }

 
          
    
   

   void OnDrawGizmos() {
     if(vertices == null) {
       return;
     }
    /* 
     for(int i = 16600; i < 16900; i++) {
       //Gizmos.DrawSphere(vertices[i], 0.1f);
       Handles.Label(vertices[i], ""+i);
     }
   */
   
   for(int i = 7300; i < 7600; i++) {
       //Gizmos.DrawSphere(vertices[i], 0.1f);
       Handles.Label(vertices[i], ""+i);
     }
   
  /*
   int i = 6894;
   for(int y = 0; y < 3; y++) {
     for(int x = 0; x < width; x++) {
      
    Handles.Label(vertices[i], ""+x);
    i++;
    
    
      
   }
   }
   */
   }
   int WaterRowLength(int y) {
         waterRowLength=0;

          for(int i = 0; i < waterTilesPerMesh.GetLength(1); i++) {
             if(waterTilesPerMesh[y,i]) {
               waterRowLength++;    
             }
  
             else if(y > 0 && waterTilesPerMesh[y-1,i]) {
               waterRowLength++;
             }
             else if( i > 0 && waterTilesPerMesh[y,i-1]) {
               waterRowLength++;
             }
             else if(i > 0 && y > 0 && !waterTilesPerMesh[y,i-1] && !waterTilesPerMesh[y-1,i] &&
                  waterTilesPerMesh[y-1,i-1]) {
               waterRowLength++;
             }
           }

           return waterRowLength;
      }





      int LandRowLength(int y) {
         landRowLength=0;

          for(int i = 0; i < landTilesPerMesh.GetLength(1); i++) {
                     
             if(landTilesPerMesh[y,i]) {
               landRowLength++;
             }
             else if(i > 0 && landTilesPerMesh[y,i-1]){
               landRowLength++;
             } 
             else if(y > 0 && landTilesPerMesh[y-1,i]) {
               landRowLength++;
             }
             else if(y > 0 && i > 0 && landTilesPerMesh[y-1,i-1]) {
               landRowLength++;
             }
           }
            return landRowLength;
      }


  public void AddMesh() { 
      Mesh mesh = new Mesh();
      GetComponent<MeshFilter>().mesh=mesh;
      mesh.Clear();
      mesh.SetVertices(vertices);
      mesh.SetTriangles(triangles,0);
      mesh.SetUVs(0,uvs);
      /*
      for(int i = 0; i < uvs.Count; i++) {
        print(i+" uvs: "+uvs[i] + " vertex: " + vertices[i]);
      }
      */
      
      
      for(int y = 0; y < height; y++) {
        for(int x = 0; x < width; x++) {
          print((x+(y*height)) + " " + uvs[x+(y*height)] + " color: " + Mesh2Noise.noiseMap[y,x]);
        }
      }
      
      mesh.RecalculateNormals();
   }
}
