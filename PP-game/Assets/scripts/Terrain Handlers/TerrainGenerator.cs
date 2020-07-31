﻿using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorCustomInspector : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate New Terrain")) {
            Terrain terrain = Selection.activeGameObject.GetComponent<Terrain>();
            new BaseTerrainGenerator().GenerateNewTerrain(
                terrain, 2, 4,
                150, .15f,
                50, 0.07f,
                25, 0.03f,
                2, 0.003f
            );
        }
    }
}

public class TerrainGenerator : MonoBehaviour { }