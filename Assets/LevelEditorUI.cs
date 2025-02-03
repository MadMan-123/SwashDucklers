using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelEditor)), CanEditMultipleObjects]
public class LevelEditorUI : Editor
{
    private LevelEditor levelEditor;
    private LevelObjectManager objectManager;
    private int selectedPrefabIndex;
    private string buffer;
    private int selectedLevelIndex;

    private void OnEnable()
    {
        levelEditor = (LevelEditor)target;
        objectManager = FindObjectOfType<LevelObjectManager>(); // Find existing instance
        levelEditor.Initialize(objectManager);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (objectManager == null || objectManager.prefabs.Count == 0)
        {
            GUILayout.Label("No prefabs found in LevelObjectManager!");
            return;
        }

        selectedPrefabIndex = EditorGUILayout.Popup("Select Prefab", selectedPrefabIndex, GetPrefabNames());

        if (GUILayout.Button("Add Level Object"))
        {
            levelEditor.AddLevelObject(objectManager.prefabs[selectedPrefabIndex].name);
        }


        
        buffer = EditorGUILayout.TextField(buffer, GUIStyle.none);
        var levelName = buffer;
        if (GUILayout.Button("New Level") && (levelName != String.Empty && levelName != "Enter the level name") )
        {
            levelEditor.NewLevel(levelName + ".levelData");
            buffer = string.Empty;
        }
        

        selectedLevelIndex = EditorGUILayout.Popup("Select Level", selectedLevelIndex, GetLevelNames());

        if (GUILayout.Button("Load Level"))
        {
            var name = GetLevelNames()[selectedLevelIndex];
            Debug.Log($"Loading {name}");
            levelEditor.LoadLevel(name);


        }

        if (GUILayout.Button("Clear level"))
        {
            levelEditor.ClearLevel();
        }
        serializedObject.ApplyModifiedProperties();
    }

    private string[] GetLevelNames()
    {
        return levelEditor.levels.ConvertAll(level => level.levelName).ToArray();
    }

    private string[] GetPrefabNames()
    {
        return objectManager.prefabs.ConvertAll(prefab => prefab.name).ToArray();
    }
}