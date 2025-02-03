using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;

public class LevelObjectManager : MonoBehaviour
{
    public List<GameObject> prefabs = new();
    private Dictionary<string, GameObject> prefabDictionary = new();
    private void OnValidate()
    {
        PopulateDictionary();
    } 
    [Editor,ExecuteAlways]
    public void PopulateDictionary()
    {
        prefabDictionary.Clear();
        foreach (var prefab in prefabs)
        {
            if (prefab != null)
            {
                prefabDictionary[prefab.name] = prefab;
            }
        }
    }

    public GameObject GetPrefabByName(string name)
    {
        if (prefabDictionary.TryGetValue(name, out var prefab))
            return prefab;

        Debug.LogWarning($"Prefab '{name}' not found in LevelObjectManager!");
        return null;
    }
}

