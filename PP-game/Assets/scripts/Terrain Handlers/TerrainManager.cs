using UnityEngine;

public static class TerrainManager {
    public static void SetTexture(TerrainData terrainData, Texture2D tex2D) {
        tex2D.Apply();

        TerrainLayer[] tex = new TerrainLayer[1];
        tex[0] = new TerrainLayer {
            diffuseTexture = tex2D,    //Sets the texture
            tileSize = new Vector2(terrainData.baseMapResolution, terrainData.baseMapResolution)    //Sets the size of the texture
        };

        terrainData.terrainLayers = tex;
    }

    public static Texture2D GetTexture(TerrainData terrainData) { return terrainData.terrainLayers[0].diffuseTexture; }

    public static Texture2D GenerateTerrainMap(float[,] heightmap) {
        Texture2D tex2D = new Texture2D(heightmap.GetLength(0), heightmap.GetLength(1));

        float maxHeight = 0;
        float minHeight = Mathf.Infinity;
        for (int x = 0; x < tex2D.width; x++) {
            for (int y = 0; y < tex2D.width; y++) {
                maxHeight = Mathf.Max(maxHeight, heightmap[x, y]);
                minHeight = Mathf.Min(minHeight, heightmap[x, y]);
            }
        }

        for (int x = 0; x < tex2D.width; x++) {
            for (int y = 0; y < tex2D.width; y++) {
                float heightOfPixel = (heightmap[x, y] - minHeight) / maxHeight;
                tex2D.SetPixel(x, y, new Color(heightOfPixel, heightOfPixel, heightOfPixel));
            }
        }

        tex2D.Apply();
        return tex2D;
    }
}
