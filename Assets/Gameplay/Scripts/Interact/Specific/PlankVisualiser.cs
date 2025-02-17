
    
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
      repairCount++;
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