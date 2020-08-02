using System.Collections.Generic;
using UnityEditor;

[System.Serializable]
public class JSONEntry {
    public int id;
    public string path;
    public string name;
}

public static class JSONManager {
    static string JSONDir = "Assets/Objects/Entities/";

    static void SaveData(List<JSONEntry> Entries) {
        string result = "[";
        foreach (JSONEntry i in Entries) {
            result += "\n\t{";
            result += "\n\t\t\"id\":"     + i.id + ",";
            result += "\n\t\t\"path\":\"" + i.path + "\",";
            result += "\n\t\t\"name\":\"" + i.name + "\"";
            result += "\n\t},";
        }

        result += "\n]";

        System.IO.File.WriteAllText(JSONDir,result);
    }
}

public class JSONEditor : EditorWindow {
    List<JSONEntry> JSON_Entries = new List<JSONEntry>();
    
    int page = 0;

    [MenuItem("Window/Custom Windows/JSON Editor")]
    static void Init() {
        // Get existing open window or if none, make a new one:
        JSONEditor window = (JSONEditor)GetWindow(typeof(JSONEditor));
        window.Show();
    }

    void OnGUI() {
        if ((page == 0) && (JSON_Entries.Count != 0)) { page = 1; }
        if ((page != 0) && (JSON_Entries.Count == 0)) { page = 0; }

        EditorGUILayout.LabelField("This window is a simple JSON interface");
        //JSONEntry e = JSON_Entries[0];








        EditorGUILayout.LabelField("Item: " + page + "/" + JSON_Entries.Count);
    }
}
