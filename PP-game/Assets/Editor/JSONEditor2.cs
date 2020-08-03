using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JSONContainer {
    public List<JSONContainer> JSONContainers = new List<JSONContainer>();
    public List<string> JSONContents = new List<string>();

    public void ListContents() { ListContents(0); }
    public void ListContents(int index) {
        Debug.Log(index);
        foreach (JSONContainer i in JSONContainers) {
            GUI.BeginGroup(new Rect (new Vector2 (0,0), new Vector2(10,10)));
            i.ListContents(index + 1);
            GUI.EndGroup();
        }

        foreach (string i in JSONContents) {
            Debug.Log("STRING: " + i);
        }
    }
}

public static class JSONManager2 {
    //WIP: stand in for later data
    public static List<JSONContainer> LoadData() {
        return new List<JSONContainer>();
    }
}

public class JSONEditor2 : EditorWindow {
    static List<JSONContainer> JSON_Entries;

    [MenuItem("Window/Custom Windows/JSON Editor2")]
    static void Init() {
        JSONEditor2 window = (JSONEditor2)GetWindow(typeof(JSONEditor2));
        window.Show();

        JSONEditor2 temp = new JSONEditor2();
        temp.Start();
    }

    void Start() {
        JSON_Entries = JSONManager2.LoadData();
        JSON_Entries = new List<JSONContainer> {
            new JSONContainer(),
            new JSONContainer(),
            new JSONContainer(),
            new JSONContainer()
        };

        JSONContainer temp = new JSONContainer();
        JSON_Entries.Add(temp);

        temp.JSONContainers.Add(new JSONContainer());
        temp.JSONContainers.Add(new JSONContainer());
        temp.JSONContainers.Add(new JSONContainer());
        temp.JSONContainers.Add(new JSONContainer());
        temp.JSONContents.Add("Testing!");
        temp.JSONContents.Add("Testing 2!");

        foreach (JSONContainer i in JSON_Entries) {
            i.ListContents();
        }
    }

    void OnGUI() {
        EditorGUILayout.LabelField("This window is a simple JSON interface");
        EditorGUILayout.Space(); EditorGUILayout.Space();

        //foreach (JSONContainer i in JSON_Entries) {
        //    i.ListContents();
        //}
    }
}
