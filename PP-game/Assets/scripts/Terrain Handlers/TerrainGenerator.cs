using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorCustomInspector : Editor {
    Terrain terrain;
    public Color ColorFieldA;
    public Color ColorFieldB;

    public override void OnInspectorGUI() {
        //do this first to make sure you have the latest version
        serializedObject.Update();

        //do this last!  it will loop over the properties on your object and apply any it needs to, no if necessary!
        serializedObject.ApplyModifiedProperties();
        
        DrawDefaultInspector();

        ColorFieldA = EditorGUILayout.ColorField(ColorFieldA);
        ColorFieldB = EditorGUILayout.ColorField(ColorFieldB);

        if (GUILayout.Button("Generate New Terrain")) {
            terrain = Selection.activeGameObject.GetComponent<Terrain>();
            BaseTerrainGenerator.GenerateNewTerrain(
                terrain,    // Reference to our Terrain Object

                //BTG.NewColor(102, 51, 0), BTG.NewColor(0, 152, 0),
                ColorFieldA, ColorFieldB,
                0, 8,       // Color Smoothing Value,               Slope Multiplier
                150, .15f,  // Perlin Noise Octave Scale 1,         Perlin Noise Octave Height 1
                50, 0.07f,  // Perlin Noise Octave Scale 2,         Perlin Noise Octave Height 2
                25, 0.03f,  // Perlin Noise Octave Scale 3,         Perlin Noise Octave Height 3
                5, 0.002f   // Perlin Noise Octave Scale 4,         Perlin Noise Octave Height 4
            );
        }
    }
}

[System.Serializable]
public class TerrainGenerator : MonoBehaviour { }
