using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorCustomInspector : Editor {
    public Color A;
    public Color B;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        A = EditorGUILayout.ColorField(A);
        B = EditorGUILayout.ColorField(B);

        if (GUILayout.Button("Generate New Terrain")) {
            Terrain terrain = Selection.activeGameObject.GetComponent<Terrain>();
            BaseTerrainGenerator.GenerateNewTerrain(
                terrain,    // Reference to our Terrain Object

                //BTG.NewColor(102, 51, 0), BTG.NewColor(0, 152, 0),
                A, B,
                0, 8,       // Color Smoothing Value,               Slope Multiplier
                150, .15f,  // Perlin Noise Octave Scale 1,         Perlin Noise Octave Height 1
                50, 0.07f,  // Perlin Noise Octave Scale 2,         Perlin Noise Octave Height 2
                25, 0.03f,  // Perlin Noise Octave Scale 3,         Perlin Noise Octave Height 3
                5, 0.002f   // Perlin Noise Octave Scale 4,         Perlin Noise Octave Height 4
            );
        }
    }
}

public class TerrainGenerator : MonoBehaviour { }
