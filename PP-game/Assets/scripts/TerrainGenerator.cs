using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorCustomInspector : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate New Terrain")) {
            new TerrainGenerator().GenerateNewTerrain(
                150, .15f,
                50, 0.07f,
                25, 0.03f,
                2, 0.003f
            );
        }
    }
}

public class TerrainGenerator : MonoBehaviour {
    public void GenerateNewTerrain(float scale_1, float height_1, float scale_2, float height_2, float scale_3, float height_3, float scale_4, float height_4) {
        //We want to actually generate the terrain now
        Terrain terrain = Selection.activeGameObject.GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;

        terrain.allowAutoConnect = true;

        int xRes = 513; int yRes = 513;
        float[,] heightmap = new float[xRes, yRes];

        float x_offset = Random.Range(0, 100);
        float y_offset = Random.Range(0, 100);

        for (int y = 0; y < yRes; y++) {
            for (int x = 0; x < xRes; x++) {
                heightmap[x, y] = Mathf.PerlinNoise(x / scale_1 + x_offset, y / scale_1 + y_offset) * height_1;
                heightmap[x, y] += Mathf.PerlinNoise(x / scale_2 + y_offset, y / scale_2 + x_offset) * height_2;
                heightmap[x, y] -= Mathf.PerlinNoise(x / scale_3 + x_offset, y / scale_3 + y_offset) * height_3;
                heightmap[x, y] += Mathf.PerlinNoise(x / scale_4 + x_offset, y / scale_4 + y_offset) * height_4;
            }
        }
        
        terrainData.SetHeights(0, 0, heightmap);
        
        //Colour in the terrain based on slope
        Texture2D tex2D = new Texture2D(xRes, yRes);
        for (int y = 0; y < terrainData.alphamapWidth; y++) {
            for (int x = 0; x < terrainData.alphamapHeight; x++) {
                tex2D.SetPixel(x, y, SlopeToColour(GetSlopeAtPosition(heightmap, x, y)));
            }
        }
        
        tex2D.Apply();
        
        //This is to convert the Texture2D that any sane person would use into the bizarre SplatPrototype array that Unity3D terrain textures use
        TerrainLayer[] tex = new TerrainLayer[1];
        tex[0] = new TerrainLayer {
            diffuseTexture = tex2D,    //Sets the texture
            tileSize = new Vector2(1, 1)    //Sets the size of the texture
        };

        terrainData.terrainLayers = tex;
        
        //Now generate the terrain map
        GenerateTerrainMap(heightmap);
    }

    float GetSlopeAtPosition(float[,] heightmap, int x, int y) {
        //Calculate the x and y slopes
        float x_slope;
        float y_slope;

        //Normally we can do this. However, we have to handle the edge case (Literally at the edge of the texture)
        try {
            x_slope =     Mathf.Abs(Mathf.Atan(heightmap[x + 1, y] / heightmap[x - 1, y]));
        } catch {
            try {
                x_slope = Mathf.Abs(Mathf.Atan(heightmap[x, y]     / heightmap[x - 1, y]));
            } catch {
                x_slope = Mathf.Abs(Mathf.Atan(heightmap[x + 1, y] / heightmap[x, y]    ));
            }
        }

        //Repeat for Y
        try {
            y_slope =     Mathf.Abs(Mathf.Atan(heightmap[x, y + 1] / heightmap[x, y - 1]));
        } catch {
            try {
                y_slope = Mathf.Abs(Mathf.Atan(heightmap[x, y]     / heightmap[x, y - 1]));
            } catch {
                y_slope = Mathf.Abs(Mathf.Atan(heightmap[x, y + 1] / heightmap[x, y]    ));
            }
        }

        //Calculate the result
        return x_slope;
    }

    Color SlopeToColour(float slope) {
        if (slope * 1000 > 0.7f) {
            return new Color(0.25f, 0, 0.75f);
        }

        return new Color(1,1,1);
    }

    void GenerateTerrainMap(float[,] heightmap) {
        Texture2D tex2D = new Texture2D(heightmap.GetLength(0), heightmap.GetLength(1));

        float maxHeight = 0;
        float minHeight = Mathf.Infinity;
        for (int x = 0; x < tex2D.width; x++) {
            for (int y = 0; y < tex2D.width; y++) {
                if (heightmap[x, y] > maxHeight) { maxHeight = heightmap[x, y]; }
                if (heightmap[x, y] < minHeight) { minHeight = heightmap[x, y]; }
            }
        }

        for (int x = 0; x < tex2D.width; x++) {
            for (int y = 0; y < tex2D.width; y++) {
                float heightOfPixel = (heightmap[x, y] - minHeight) / maxHeight;
                tex2D.SetPixel(x, y, new Color(heightOfPixel, heightOfPixel, heightOfPixel));
            }
        }

        tex2D.Apply();
        System.IO.File.WriteAllBytes("Assets/graphics/Terrain Map.jpg", tex2D.EncodeToJPG());
    }
}
