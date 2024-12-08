using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NOTE: READ ALL OF GERALD'S CODE IN COMIC BOOK GUY'S VOICE
/// GM: as you can see, I have made a bunch of bullshit here that doesn't really work, need to fix it, please expect comments of me detailing my increase of hairloss due to stress
/// dealing with this shit. Anyway for my own sake of mind what I need to do is the following:
/// 1. Reference inventory.cs so i can have the inventory parameters set up correctly
/// 2. make sure the canonBall transfers to the canon's inventory
/// 3. check with folk on how code looks
/// 4. drink coffee
/// 5. invest in tupee before friday
/// </summary>


/// <summary>
/// GM: Manages the functionality of loading cannonballs into the cannon.
/// GM: Handles inventory checks, transfers, and ensuring proper operation.
/// </summary>
public class CannonManager : MonoBehaviour
{
    // GM: Reference to the cannon's inventory (can hold one item, for simplicity).
    private Item cannonBall;

    // GM: Reference to the player inventory.
    [SerializeField] private Inventory playerInventory;

    // GM: Reference to the interact range for the cannon.
    [SerializeField] private float interactRange = 2f;

    /// <summary>
    /// GM: Handles the interaction logic for transferring a CannonBall from the player to the cannon.
    /// </summary>
    /// 
    public void InteractWithCannon()
    {

        // GM: Check if the player is within range of the cannon.
        if (!IsPlayerInRange())
        {
            Debug.Log("Player is too far from the cannon.");
            return;
        }

        // GM: Check if the player is holding a CannonBall.
        Item playerItem = playerInventory.GetItem();
        if (playerItem == null || playerItem.type != ItemManager.Type.CannonBall)
        {
            Debug.Log("Player is not holding a CannonBall.");
            return;
        }

        // GM: Check if the cannon is already loaded.
        if (cannonBall != null)
        {
            Debug.Log("Cannon is already loaded!");
            return;
        }

        // GM: Transfer the CannonBall from the player to the cannon.
        LoadCannon(playerItem);
    }
     
    /// <summary>
    /// GM: Transfers the CannonBall from the player to the cannon.
    /// </summary>
    /// <param name="item">The item to transfer.</param>
    private void LoadCannon(Item item)
    {
        // GM: Remove the CannonBall from the player's inventory.
        playerInventory.RemoveItem();

        // GM: Add the CannonBall to the cannon's inventory.
        cannonBall = item;

        Debug.Log("CannonBall loaded into the cannon!");
    }


    /// <summary>
    /// GM: Fires the cannon and clears the inventory.
    /// </summary>
    public void FireCannon()
    {
        if (cannonBall == null)
        {
            Debug.Log("Cannon is empty! Load a CannonBall first.");
            return;
        }

        // GM: Perform firing logic (to be expanded as needed).
        Debug.Log("Firing cannon!");

        // GM: Reset the cannon's inventory.
        cannonBall = null;
    }

    /// <summary>
    /// GM: Checks if the player is within range to interact with the cannon.
    /// </summary>
    /// <returns>True if the player is within range; otherwise, false.</returns>
    private bool IsPlayerInRange()
    {
        return Vector3.Distance(playerInventory.transform.position, transform.position) <= interactRange;
    }

    /// <summary>
    /// GM: I've made different changes to this code, but FML nothing works.
    /// </summary>
}
