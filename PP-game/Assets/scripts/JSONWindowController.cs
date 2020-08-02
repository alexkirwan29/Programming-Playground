using UnityEditor;

public class JSONWindowController : EditorWindow {
    
    [MenuItem("Window/JSON Editor")]
    static void Init() {
        // Get existing open window or if none, make a new one:
        JSONWindowController window = (JSONWindowController)GetWindow(typeof(JSONWindowController));
        window.Show();
    }


}
