
    
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlankVisualiser : MonoBehaviour
{
   //New idea, Instantiate an instance of the planks so we can move it later, also i cant get it to rotate for some reason cos im a dumbass - TS
   
   
   //order of visuals is the order of how the planks are placed
   [SerializeField] private GameObject plankVisuals;
   [SerializeField] GameObject[] plankPool= new GameObject[2];
   [SerializeField] private Leak leak;
   [SerializeField] private GameObject spawnLoc;
   private float firstRotation;
   public Transform vfxHolder;
   private int toRepair = 0;


   private void Start()
   {
      leak = gameObject.GetComponent<Leak>();
      vfxHolder = leak.vfxHolder;
      for (int i = 0; i < plankPool.Length; i++)
      {
         plankPool[i] = Instantiate(plankVisuals,spawnLoc.transform.position,Quaternion.Euler(0,0,0),vfxHolder);
         plankPool[i].SetActive(false);
      }
      
   }

   public void LeakSpawn(int num)
   {
      toRepair = num;
      
      for (int i = 0; i < plankPool.Length; i++)
      {
            plankPool[i].SetActive(false);
      }
   }
   public void RepairPlank(int count)
   {
      //if (repairCount >= toRepair+1) return;

      float yRot = Random.Range(0, 180);
      Quaternion rotation = (Quaternion.Euler(0, yRot, 0));
      if (count == toRepair)
      {
         rotation = (Quaternion.Euler(0, firstRotation+90f, 0));
      }
      firstRotation = yRot;
      plankPool[count].transform.rotation = rotation;
      plankPool[count].SetActive(true);
   }
   
   //on disable reset the visuals
   private void OnDisable()
   {
      //foreach (var plankVisual in plankVisuals)
      //{
      //   plankVisual.SetActive(false);
      //}
      //repairCount = 0;
   }



}