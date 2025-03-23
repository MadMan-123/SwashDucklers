
    
using UnityEngine;

public class PlankVisualiser : MonoBehaviour
{
   //order of visuals is the order of how the planks are placed
   [SerializeField] private GameObject[] plankVisuals;
   public int repairCount = 0;
   
   public void RepairPlank()
   {
      if (repairCount >= plankVisuals.Length) return;
      plankVisuals[repairCount].SetActive(true);
      plankVisuals[repairCount].transform.rotation *= Quaternion.Euler(0, Random.Range(0, 180f), 0);
      repairCount++;
   }

   public void RemovePlank()
   {
      foreach (var plankVisual in plankVisuals)
      {
         plankVisual.SetActive(false);
      }
      repairCount = 0;
   }
   
   //on disable reset the visuals
   private void OnDisable()
   {
      foreach (var plankVisual in plankVisuals)
      {
         plankVisual.SetActive(false);
      }
      repairCount = 0;
   }


}