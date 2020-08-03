﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class JSONEntry {
    public int id;
    public string path;
    public string name;
}

public static class JSONManager {
    static string JSONDir = "Assets/Objects/Entities/net-prefab-list.json";

    public static void SaveData(List<JSONEntry> Entries) {
        string result = "[";
        foreach (JSONEntry i in Entries) {
            result += "\n\t{";
            result += "\n\t\t\"id\":"     + i.id + ",";
            result += "\n\t\t\"path\":\"" + i.path + "\",";
            result += "\n\t\t\"name\":\"" + i.name + "\"";
            result += "\n\t},";
        }

        System.IO.File.WriteAllText(JSONDir, result.Substring(0, result.Length - 1) + "\n]");
        //Debug.Log(result.Substring(0, result.Length - 1) + "\n]");
    }

    public static List<JSONEntry> LoadData() {
        List<JSONEntry> JSON_Entries = new List<JSONEntry>();

        bool readingData = false;
        JSONEntry currentEntry = new JSONEntry();
        foreach (string i in System.IO.File.ReadLines(JSONDir)) {
            if ((!readingData) && i.Contains("{")) { readingData = true; }
            else if (readingData && i.Contains("}")) { readingData = false; }

            if (readingData) {
                if (i.ToLower().Contains("\"id\":")) {
                    string temp = i.Substring(i.IndexOf(":") + 1, i.Length - i.IndexOf(":") - 2);
                    currentEntry.id = int.Parse(temp);
                }

                if (i.ToLower().Contains("\"path\":")) {
                    int startingIndex = i.IndexOf("\"", i.IndexOf(":")) + 1;
                    int finishedIndex = i.LastIndexOf("\"") - startingIndex;
                    currentEntry.path = i.Substring(startingIndex, finishedIndex);
                }

                if (i.ToLower().Contains("\"name\":")) {
                    int startingIndex = i.IndexOf("\"", i.IndexOf(":")) + 1;
                    int finishedIndex = i.LastIndexOf("\"") - startingIndex;
                    currentEntry.name = i.Substring(startingIndex, finishedIndex);
                }
            } else {
                if (currentEntry.name != null) { JSON_Entries.Add(currentEntry); }
                currentEntry = new JSONEntry();
            }
        }
        
        return JSON_Entries;
    }
}

public class JSONEditor : EditorWindow {
    static List<JSONEntry> JSON_Entries;
    int page = 0;

    [MenuItem("Window/Custom Windows/JSON Editor")]
    static void Init() {
        JSONEditor window = (JSONEditor)GetWindow(typeof(JSONEditor));
        window.Show();

        JSONEditor temp = new JSONEditor();
        temp.Start();
    }

    void Start() {
        JSON_Entries = JSONManager.LoadData();
        if (JSON_Entries == null) {
            JSON_Entries = new List<JSONEntry>();
        }
    }

    void OnGUI() {
        EditorGUILayout.LabelField("This window is a simple JSON interface");
        EditorGUILayout.Space(); EditorGUILayout.Space();

        //Save data line
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Data")) { JSONManager.SaveData(JSON_Entries); }
        GUILayout.EndHorizontal();

        //Array modifier lines
        GUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(JSON_Entries.Count == 0);
        if (GUILayout.Button("Delete Page") && (JSON_Entries.Count != 0)) { JSON_Entries.RemoveAt(page - 1); }
        EditorGUI.EndDisabledGroup();

        if (GUILayout.Button("Add Page")) {
            JSON_Entries.Add(new JSONEntry { id = 0, path = "Null", name = "Null" });
            page = JSON_Entries.Count;
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Space(); EditorGUILayout.Space();

        //Page logic
        if ((page == 0) && (JSON_Entries.Count != 0)) { page = 1; }
        if ((page != 0) && (JSON_Entries.Count == 0)) { page = 0; }
        if (page > JSON_Entries.Count) { page = JSON_Entries.Count; }

        //JSONEntry e = JSON_Entries[0];
        if (JSON_Entries.Count != 0) {
            JSON_Entries[page - 1].id =   EditorGUILayout.IntField("ID",    JSON_Entries[page - 1].id);
            JSON_Entries[page - 1].path = EditorGUILayout.TextField("Path", JSON_Entries[page - 1].path);
            JSON_Entries[page - 1].name = EditorGUILayout.TextField("Name", JSON_Entries[page - 1].name);
        }

        EditorGUILayout.Space(); EditorGUILayout.Space();

        //Page selector lines
        GUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup((page == 0) || (page == 1));
        if (GUILayout.Button("Previous Page")) { page--; }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.LabelField("Item: " + page + "/" + JSON_Entries.Count);

        EditorGUI.BeginDisabledGroup((JSON_Entries.Count == 0) || (page == JSON_Entries.Count));
        if (GUILayout.Button("Next Page")) { page++; }
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();
        
    }
}