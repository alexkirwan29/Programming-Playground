using UnityEngine;

public static class BaseTerrainGenerator {
    public static void GenerateNewTerrain(Terrain terrain, int smoothing, float slopeMultiplier, float scale_1, float height_1, float scale_2, float height_2, float scale_3, float height_3, float scale_4, float height_4) {
        //We want to actually generate the terrain now
        TerrainData terrainData = terrain.terrainData;

        int xRes = 513; int yRes = 513;
        float[,] heightmap = new float[xRes, yRes];

        float x_offset = Random.Range(0, 100);
        float y_offset = Random.Range(0, 100);

        for (int y_pos = 0; y_pos < yRes; y_pos++) {
            for (int x_pos = 0; x_pos < xRes; x_pos++) {
                heightmap[x_pos, y_pos] = Mathf.PerlinNoise(x_pos / scale_1 + x_offset, y_pos / scale_1 + y_offset) * height_1;
                heightmap[x_pos, y_pos] += Mathf.PerlinNoise(x_pos / scale_2 + y_offset, y_pos / scale_2 + x_offset) * height_2;
                heightmap[x_pos, y_pos] -= Mathf.PerlinNoise(x_pos / scale_3 + x_offset, y_pos / scale_3 + y_offset) * height_3;
                heightmap[x_pos, y_pos] += Mathf.PerlinNoise(x_pos / scale_4 + x_offset, y_pos / scale_4 + y_offset) * height_4;
            }
        }

        terrainData.SetHeights(0, 0, heightmap);

        //Colour in the terrain based on slope
        Texture2D tex2D = new Texture2D(513, 513);
        for (int y = 0; y < terrainData.alphamapResolution; y++) {
            for (int x = 0; x < terrainData.alphamapResolution; x++) {
                tex2D.SetPixel(y, x, SlopeToColour(GetAverageSlopeAtPosition(heightmap, x, y, smoothing) * slopeMultiplier));
            }
        }

        //This is to convert the Texture2D that any sane person would use into the bizarre TerrainLayer array that Unity3D terrain textures use
        TerrainManager.SetTexture(terrainData, tex2D);

        //Now generate the terrain map
        System.IO.File.WriteAllBytes("Assets/graphics/Terrain Map.jpg",TerrainManager.GenerateTerrainMap(heightmap).EncodeToJPG());
    }

    static float GetAverageSlopeAtPosition(float[,] heightmap, int x, int y, int smoothing) {
        if (smoothing == 0) { return GetSlopeAtPosition(heightmap, x, y); }

        float numberOfPoints = 0, result = 0;
        for (int x_offset = -smoothing; x_offset < smoothing; x_offset++) {
            for (int y_offset = -smoothing; y_offset < smoothing; y_offset++) {
                try {
                    result += GetSlopeAtPosition(heightmap, x + x_offset, y + y_offset);
                    numberOfPoints++;
                } catch { }
            }
        }
        return result / numberOfPoints;
    }

    static float GetSlopeAtPosition(float[,] heightmap, int x_pos, int y_pos) {
        float min = 0, max = 0;
        
        try {           min = Mathf.Min(heightmap[x_pos, y_pos], heightmap[x_pos + 1, y_pos], heightmap[x_pos, y_pos + 1], heightmap[x_pos + 1, y_pos + 1]);
        } catch { try { min = Mathf.Min(heightmap[x_pos, y_pos], heightmap[x_pos - 1, y_pos], heightmap[x_pos, y_pos + 1], heightmap[x_pos - 1, y_pos + 1]);
        } catch { try { min = Mathf.Min(heightmap[x_pos, y_pos], heightmap[x_pos + 1, y_pos], heightmap[x_pos, y_pos - 1], heightmap[x_pos + 1, y_pos - 1]);
        } catch { try { min = Mathf.Min(heightmap[x_pos, y_pos], heightmap[x_pos - 1, y_pos], heightmap[x_pos, y_pos - 1], heightmap[x_pos - 1, y_pos - 1]);
        } catch { try { min = heightmap[x_pos, y_pos];
        } catch { throw new System.Exception("Position Outside of Heightmap Bounds!"); } } } } }

        try { max = Mathf.Max(heightmap[x_pos, y_pos], heightmap[x_pos + 1, y_pos], heightmap[x_pos, y_pos + 1], heightmap[x_pos + 1, y_pos + 1]);
        } catch { try { max = Mathf.Max(heightmap[x_pos, y_pos], heightmap[x_pos - 1, y_pos], heightmap[x_pos, y_pos + 1], heightmap[x_pos - 1, y_pos + 1]);
        } catch { try { max = Mathf.Max(heightmap[x_pos, y_pos], heightmap[x_pos + 1, y_pos], heightmap[x_pos, y_pos - 1], heightmap[x_pos + 1, y_pos - 1]);
        } catch { try { max = Mathf.Max(heightmap[x_pos, y_pos], heightmap[x_pos - 1, y_pos], heightmap[x_pos, y_pos - 1], heightmap[x_pos - 1, y_pos - 1]);
        } catch { try { max = heightmap[x_pos, y_pos];
        } catch { throw new System.Exception("Position Outside of Heightmap Bounds!");  } } } } }

        return Mathf.Atan((max - min) / Mathf.Sqrt(2)) * Mathf.Rad2Deg;
    }

    //Color SlopeToColour(float slope) { return BlendColour(new Color(0,152,0), new Color(102,51,0), slope); }
    static Color SlopeToColour(float slope) { return BlendColour(NewColor(102,51,0), NewColor(0, 152, 0), slope); }

    static Color BlendColour(Color A, Color B, float t) {
        //t = Mathf.Sqrt(t);
        return new Color() {
            r = A.r * t + B.r * (1 - t),
            g = A.g * t + B.g * (1 - t),
            b = A.b * t + B.b * (1 - t),
            a = A.a * t + B.a * (1 - t)
        };
    }

    static float[,] HeightmapFromTerrainData(TerrainData terrainData) {
        return terrainData.GetHeights(0, 0, terrainData.heightmapResolution - 1, terrainData.heightmapResolution - 1);
    }

    static Color NewColor(int r, int g, int b) { return NewColor(r, g, b, 255); }
    static Color NewColor(int r, int g, int b, int a) { return new Color((float)r / 255, (float)g / 255, (float)b / 255, (float)a / 255); }
}