using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private Transform levelParent;  // Parent container for organization
    private readonly List<LevelObject> objects = new();
    private LevelObjectManager objectManager;
    public List<LevelData> levels = new();
    private void LoadAllLevels()
    {
        string path = Application.persistentDataPath;
        string[] files = Directory.GetFiles(path);

        levels.Clear(); // Clear previous data

        foreach (string file in files)
        {
            LevelData levelData = SaveSystem.LoadDataRaw<LevelData>(file); 
            if (levelData != null)
            {
                levels.Add(levelData);
            }
        }

        Debug.Log($"Loaded {levels.Count} level(s) from {path}");
    }

    private void OnValidate()
    {
        //try get all levels
        LoadAllLevels();
    }

    public void Initialize(LevelObjectManager manager)
    {
        objectManager = manager;
        objectManager.PopulateDictionary();
    }

    public LevelObject AddLevelObject(string prefabName)
    {
        if (objectManager == null)
        {
            Debug.LogError("LevelObjectManager is not assigned");
            return null;
        }

        GameObject prefab = objectManager.GetPrefabByName(prefabName);
        if (prefab == null) return null;

        // Use Instantiate for scene editing
        GameObject newObject = Instantiate(prefab, levelParent ? levelParent : transform, true);
        newObject.name = prefab.name;
        newObject.transform.position = Vector3.zero;

        LevelObject levelObject = newObject.AddComponent<LevelObject>();
        levelObject.prefabName = prefabName;
        objects.Add(levelObject);
        return levelObject;
    }

    public void NewLevel(string name)
    {
        //check if the level alraedy exists
        var data = SaveSystem.LoadData<LevelData>(name);
        if (data != null)
        {
            Debug.LogError($"Level {name} Already Exists at {SaveSystem.GetFullPath(name)}");
            return;
        }
        LevelData newData = new();
        //setup arrays
        newData.x = new float[objects.Count];
        newData.y = new float[objects.Count];
        newData.z = new float[objects.Count];
        newData.prefabNames = new string[objects.Count];
        
        for (int i = 0; i < objects.Count; i++)
        {
            var o = objects[i];
            if(o == null) continue;
            var position = o.transform.position;
            newData.x[i] = position.x;
            newData.y[i] = position.y;
            newData.z[i] = position.z;
            newData.prefabNames[i] = objects[i].prefabName;
            newData.count++;
        }

        newData.levelName = name;
        
        levels.Add(newData);
        
        SaveSystem.SaveData(newData, name);
    }


    
    public void ClearLevel()
    {
        foreach (var obj in objects)
        {
            DestroyImmediate(obj.gameObject);
        }
        objects.Clear();
    }

    [Serializable]
    public class LevelData
    {
        public int count;
        public string levelName;
        public float[] x;
        public float[] y;
        public float[] z;
        public string[] prefabNames;
    }

    public void LoadLevel(string getLevelName)
    {
        //clear the current level
        ClearLevel();
       
        //load data
        var data = SaveSystem.LoadData<LevelData>(getLevelName);
        if (data == null)
        {
            Debug.LogError($"No level called: {getLevelName} exists");
        }
       
        
        //go through all the objects positions and spawn objects
        for (int i = 0; i < data.count; i++)
        {
            var obj = AddLevelObject(data.prefabNames[i]);

            obj.transform.position = new Vector3(data.x[i], data.y[i], data.z[i]);
        }
        
        Debug.Log("Level Loaded");
    }
}