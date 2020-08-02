using UnityEditor;

struct JSONEntry {
    int id;
    string path;
    string name;
}

public class JSONWindowController : EditorWindow {
    string JSONDir = "";

    [MenuItem("Window/Custom Windows/JSON Editor")]
    static void Init() {
        // Get existing open window or if none, make a new one:
        JSONWindowController window = (JSONWindowController)GetWindow(typeof(JSONWindowController));
        window.Show();
    }

    void OnGUI() {
        EditorGUILayout.LabelField("This window is a simple JSON interface");
        JSONEntry s;
    }
}
