using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorCustomInspector : Editor {
    int selected = 0;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate New Terrain")) {
            TerrainGenerator tg = new TerrainGenerator();
            tg.GenerateNewTerrain(
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

        int xRes = 513; int yRes = 513;
        float[,] heightmap = new float[xRes, yRes];

        float x_offset = UnityEngine.Random.Range(0,100);
        float y_offset = UnityEngine.Random.Range(0,100);

        for (int y = 0; y < yRes; y++) {
            for (int x = 0; x < xRes; x++) {
                //Height map 1
                heightmap[x, y] = Mathf.PerlinNoise(x / scale_1 + x_offset, y / scale_1 + y_offset) * height_1;

                //Height map 2
                heightmap[x, y] += Mathf.PerlinNoise(x / scale_2 + y_offset, y / scale_2 + x_offset) * height_2;

                //Height map 3
                heightmap[x, y] -= Mathf.PerlinNoise(x / scale_3 + x_offset, y / scale_3 + y_offset) * height_3;

                //Height map 4
                heightmap[x, y] += Mathf.PerlinNoise(x / scale_4 + x_offset, y / scale_4 + y_offset) * height_4;
            }
        }

        terrainData.SetHeights(0, 0, heightmap);
    }
}
