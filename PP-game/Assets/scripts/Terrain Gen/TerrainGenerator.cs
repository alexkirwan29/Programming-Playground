using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorCustomInspector : Editor {
    TerrainGenerator tg;

    public override void OnInspectorGUI() {
        if (GUILayout.Button("Generate New Terrain")) {
            if (tg == null)
                tg = new TerrainGenerator();
            tg.GenerateNewTerrain();
        }
    }
}

public class TerrainGenerator : MonoBehaviour {
    public void GenerateNewTerrain() {
        //We want to actually generate the terrain now
        Terrain terrain = Selection.activeGameObject.GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;

        int xRes = 513; int yRes = 513;
        float[,] heightmap = new float[xRes, yRes];

        float scale_1 = 100.01f;
        float scale_2 = 10.01f;
        float scale_3 = 1.01f;
        for (int y = 0; y < yRes; y++) {
            for (int x = 0; x < xRes; x++) {
                heightmap[x, y] = Mathf.PerlinNoise(x / scale_1, y / scale_1) * 0.25f;
                heightmap[x, y] += Mathf.PerlinNoise(x / scale_2, y / scale_2) * 0.1f;
                heightmap[x, y] += Mathf.PerlinNoise(x / scale_3, y / scale_3) * 0.01f;
            }
        }

        terrainData.SetHeights(0, 0, heightmap);
    }
}
