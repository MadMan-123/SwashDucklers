using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class AreaManager : MonoBehaviour
{
        //buffer of all areas to be added to the map
        [SerializeField] private List<Area> areas = new();
        //areas hash map
        private static readonly Dictionary<string, Area> areaMap = new();
        //singleton
        public static AreaManager instance;

        private void Awake()
        {
                //singleton boilerplate 
                if(instance == null)
                        instance = this;
                else
                        Destroy(gameObject);
                
                //add to hash map
                 for (var i = 0; i < areas.Count; i++)
                 {
                         areaMap.Add(areas[i].name, areas[i]);
                         Debug.Log("Added area: " + areas[i].name);
                 } 
        }

        private void Start()
        {
               
        }
        //get area with string name
        public static Area GetArea(string name) => areaMap[name];
        
        //definition of an area
        [Serializable]
        public class Area
        {
                public string name;
                public Transform Min, Max;
                
                //basically a collision check
                public bool IsInArea(Vector3 pos) => (pos.x >= Min.position.x && pos.x <= Max.position.x) && 
                                                     (pos.y >= Min.position.y && pos.y <= Max.position.y) && 
                                                     (pos.z >= Min.position.z && pos.z <= Max.position.z);

                //the mod part takes into account if we want a random value on each axis
                public Vector3 GeneratePositionInArea(bool xMod = true, bool yMod = false, bool zMod = true)
                {
                        
                        Vector3 pos = new();
                        if(xMod)
                               pos.x = (int)Random.Range(Min.position.x, Max.position.x);
                        if(yMod)
                                pos.y = (int)Random.Range(Min.position.y, Max.position.y);
                        else
                                pos.y = (int)Min.position.y;
                        if(zMod)
                                pos.z = (int)Random.Range(Min.position.z, Max.position.z);
                        //generate a random position in the area
                        return pos;

                }
                
                public Vector3 ClipToFloor(Vector3 pos)
                {
                        //clip the position to the floor
                        //take the position and sphere cast to find a valid point 
                        RaycastHit[] colliders = new RaycastHit[10];
                        int result = Physics.SphereCastNonAlloc(pos, 0.5f, Vector3.down, colliders);

                        if (result == 0)
                        {
                                //return the min position y axis
                                pos.y = Min.position.y;
                                return pos;
                        }
                        //return the hit point
                        return colliders[0].point;
                }
        }


    private void OnDrawGizmos()
    {
                //draw the areas of each "Area" and draw text above to tell this information
            for (int i = 0; i < areas.Count; i++)
            {
                   var area = areas[i];
                   if((!area.Min && !area.Max)) continue;
                   //draw the area
                   Gizmos.color = Color.green;
                   // Calculate the center and size of the cube
                   Vector3 center = (area.Min.position + area.Max.position ) / 2;
                   Vector3 size = area.Max.position - area.Min.position;;
                   Gizmos.DrawWireCube(center, size);
                   //draw the name
                   Handles.Label(center + transform.up * size.y, area.name);
            }
    }

}
