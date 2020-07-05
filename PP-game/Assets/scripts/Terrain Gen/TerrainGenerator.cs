using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorInspector : Editor {
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
    GameObject terrain;
    
    public void GenerateNewTerrain() {
        print("Hello!");
        if (terrain != null)
            Destroy(terrain);

        //We want to actually generate the terrain now
        float[,] heightmap = new float[,];

        TerrainData terraindata = new TerrainData();
        terraindata.SetHeights(0,0,);
        terrain = Terrain.CreateTerrainGameObject(terraindata);
    }
}
