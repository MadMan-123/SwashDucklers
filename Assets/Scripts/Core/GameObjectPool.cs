using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core
{
        //Simple object pool system - MW
        public class GameObjectPool 
        {
            private List<GameObject> objects = new List<GameObject>();
            private GameObject prefab;

            public int Count => objects.Count;
            private Transform pTransform;

            public bool Dynamic = true;
            public bool SetActiveOnGet = true;

            // Constructor to initialize the pool with a prefab, initial size, and parent transform
            public GameObjectPool(GameObject prefab, int initialSize, Transform parent)
            {
                this.prefab = prefab;
                if (parent != null)
                    pTransform = parent;

                // Create the initial pool of objects
                for (int i = 0; i < initialSize; i++)
                {
                    CreateObjectInPool();
                }
                var str =  prefab ? prefab.name : "Empty Object";
                Debug.Log($"Pool Created: {str}, Pool Size: {objects.Count}");
            }
        
       

            // Create a new object in the pool and add it to the list
            private GameObject CreateObjectInPool()
            {

                GameObject obj = null;
                if(prefab)
                    obj = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity, pTransform ? pTransform : null);

                obj.SetActive(false);
                // Give the object the "Pooled" tag
                obj.tag = "Pooled";
                objects.Add(obj);
                return obj;
            }
            public GameObject CreateObjectInPool(GameObject newPrefab, Transform newTransformParent = null)
            {

                GameObject obj;
                obj = Object.Instantiate(newPrefab, Vector3.zero, Quaternion.identity, newTransformParent ? newTransformParent : null);

                obj.SetActive(false);
                // Give the object the "Pooled" tag
                obj.tag = "Pooled";
                objects.Add(obj);
                return obj;
            }
        

            // Get an inactive object from the pool
            public GameObject GetObject()
            {
                for (var index = 0; index < objects.Count; index++)
                {
                    var obj = objects[index];
                    if (obj.activeInHierarchy) continue;
                    if (SetActiveOnGet)
                        obj.SetActive(true);
                    return obj;
                }

                if (!Dynamic) return null;
                // If all objects are in use, create a new one and add it to the pool
                GameObject newObj = CreateObjectInPool();
#if UNITY_EDITOR
                Debug.Log($"Pool Added: {newObj.name}, Pool Size: {objects.Count} : +1");
#endif
                newObj.SetActive(true);
                return newObj;

            }
        
            public GameObject GetObject(GameObject obj)
            {
                //get the object from the pool
                var current = objects.Find(x => obj);
            
                if (SetActiveOnGet)
                    current.SetActive(true);
            
                return current;
            

            }
        
        
            public List<GameObject> GetAllObjects()
            {
                return objects;
            }
            
            public void SetBuffer(List<GameObject> buffer) => objects = buffer;
            
            // Return an object to the pool by deactivating it
            public void ReturnObject(GameObject obj)
            {

                obj.SetActive(false);
            }

            // Clean up function
            void CleanUp()
            {
                objects.Clear();
            
                GC.Collect();
            }
        
            // Indexer to access elements of the pool list
            public GameObject this[int index]
            {
                get
                {
                    if (index < 0 || index >= objects.Count)
                        throw new IndexOutOfRangeException("Index is out of range for ObjectPool");

                    return objects[index];
                }
                set
                {
                    if (index < 0 || index >= objects.Count)
                        throw new IndexOutOfRangeException("Index is out of range for ObjectPool");

                    objects[index] = value;
                }
            }
            public GameObjectPool(int size, Transform parent)
            {
                prefab = null;
                objects = new List<GameObject>(size);
            
                if (parent != null)
                    pTransform = parent;

                // Create empty buffer of GameObjects
                for (int i = 0; i < size; i++)
                {
                    GameObject obj = new GameObject($"Empty_{i}");
                    obj.transform.SetParent(pTransform);
                    obj.SetActive(false);
                    obj.tag = "Pooled";
                    objects.Add(obj);
                }
            
                Debug.Log($"Empty Buffer Created, Size: {objects.Count}");
            }
 
            // Implementation of IEnumerable


            public bool Contains(GameObject objectToReturn)
            {
                return objects.Contains(objectToReturn);
            }
        }
    }

