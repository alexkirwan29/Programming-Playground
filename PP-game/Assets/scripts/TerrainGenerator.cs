using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorCustomInspector : Editor {
    int selected = 0;

    public override void OnInspectorGUI() {
        string[] options = new string[] { "Standard Land", "Standard Island" };
        selected = EditorGUILayout.Popup("Label", selected, options);

        DrawDefaultInspector();

        if (GUILayout.Button("Generate New Terrain")) {
            TerrainGenerator tg = new TerrainGenerator();
            tg.GenerateNewTerrain(
                selected,
                150, .15f,
                50, 0.07f,
                25, 0.03f
            );
        }
    }
}

public class TerrainGenerator : MonoBehaviour {
    public void GenerateNewTerrain(int landType, float scale_1, float height_1, float scale_2, float height_2, float scale_3, float height_3) {
        //We want to actually generate the terrain now
        Terrain terrain = Selection.activeGameObject.GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;

        int xRes = 513; int yRes = 513;
        float[,] heightmap = new float[xRes, yRes];

        float x_offset = UnityEngine.Random.Range(0,100);
        float y_offset = UnityEngine.Random.Range(0,100);

        float lowestPoint = Mathf.Infinity;
        float highestPoint = 0;

        for (int y = 0; y < yRes; y++) {
            for (int x = 0; x < xRes; x++) {
                //Height map 1
                heightmap[x, y] = Mathf.PerlinNoise(x / scale_1 + x_offset, y / scale_1 + y_offset) * height_1;

                //Height map 2
                heightmap[x, y] += Mathf.PerlinNoise(x / scale_2 + y_offset, y / scale_2 + x_offset) * height_2;

                //Height map 3
                heightmap[x, y] -= Mathf.PerlinNoise(x / scale_3 + x_offset, y / scale_3 + y_offset) * height_3;

                //Island calculations for height map correction
                float x2 = (float)Convert.ToDouble(x) - 250;
                float y2 = (float)Convert.ToDouble(y) - 250;
                float distFromCenter = Mathf.Sqrt(Mathf.Pow(x2, 2) + Mathf.Pow(y2, 2));

                switch (landType) {
                    case 1:
                        heightmap[x, y] *= cosNotRepeatingCorrected(distFromCenter / 90);
                        break;
                }

                if (heightmap[x, y] < lowestPoint)
                    lowestPoint = heightmap[x, y];
                if (heightmap[x, y] > highestPoint)
                    highestPoint = heightmap[x, y];
            }
        }

        Transform go = null;
        try { go = Selection.activeGameObject.transform.Find("Water Level"); } catch { }

        if (go == null) {
            go = GameObject.CreatePrimitive(PrimitiveType.Plane).transform;
            go.localScale = new Vector3(100, 1, 100);
            go.name = "Water Level";
            go.SetParent(Selection.activeGameObject.transform);
        }
        
        terrainData.SetHeights(0, 0, heightmap);

        print("Max: " + highestPoint + "      Min: " + lowestPoint);
        float waterHeight = getPercentageOfHeight(highestPoint, lowestPoint, 0.25f);
        float coastlineHeight = getPercentageOfHeight(highestPoint, lowestPoint, 0.2f);
        float grassHeight = getPercentageOfHeight(highestPoint, lowestPoint, 0.3f);
        float mountainsHeight = getPercentageOfHeight(highestPoint, lowestPoint, 0.5f);
        float mountainTopHeight = getPercentageOfHeight(highestPoint, lowestPoint, 0.85f);

        for (int y = 0; y < yRes; y++) {
            for (int x = 0; x < xRes; x++) {
                //We want to edit the texture based on height

            }
        }

        go.position = new Vector3(500, waterHeight, 500);
    }

    public float cosNotRepeatingCorrected(float x) {
        return Mathf.Cos(Mathf.Clamp(x,0,Mathf.PI))/2 + 0.5f;
    }

    public float getPercentageOfHeight(float max, float min, float percentage){
        return (max - min) * percentage + min;
    }
}
