using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections; 
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
      StartCoroutine(PopOff());
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
      plankPool[count].transform.position = spawnLoc.transform.position;
      plankPool[count].SetActive(true);
   }

   public IEnumerator PopOff()
   {
      for (int i = 0; i < plankPool.Length; i++)
      {
         if (plankPool[i].TryGetComponent(out Rigidbody rb))
         {
            float ranX = Random.Range(0.75f, 0);
            float ranZ = Random.Range(0.75f, 0);
            rb.isKinematic = false;
            //rb.AddTorque(ranX*20,ranZ*20,ranZ*20, ForceMode.Impulse);
            rb.AddForce((Vector3.up + new Vector3(ranX,0,ranZ)) * 15, ForceMode.Impulse);
         }
      }
      yield return new WaitForSeconds(1);
      for (int i = 0; i < plankPool.Length; i++)
      {
         if (plankPool[i].TryGetComponent(out Rigidbody rb))
         {
            rb.velocity = Vector3.zero;
         }
         plankPool[i].SetActive(false);
      }
      
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