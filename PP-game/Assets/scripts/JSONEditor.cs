using System.Collections.Generic;
using UnityEditor;

struct JSONEntry {
    int id;
    string path;
    string name;
}

public class JSONEditor : EditorWindow {
    string JSONDir = "";
    List<JSONEntry> JSON_Entries = new List<JSONEntry>();

    [MenuItem("Window/Custom Windows/JSON Editor")]
    static void Init() {
        // Get existing open window or if none, make a new one:
        JSONEditor window = (JSONEditor)GetWindow(typeof(JSONEditor));
        window.Show();
    }

    void OnGUI() {
        EditorGUILayout.LabelField("This window is a simple JSON interface");
        JSONEntry s;
    }
}
