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
    [SerializeField] private Transform cannonBallSlot; // Where the cannonball will visually go in the cannon.

    [SerializeField] KrakenManager kraken; //Reference to the kraken

    /// <summary>
    /// GM: Handles loading a cannonball into the cannon and deletes it from the world.
    /// </summary>
    /// <param name="playerInventory">The player's inventory script reference.</param>
    public void LoadCannon(Inventory playerInventory)
    {
        // GM: Check if the player's inventory is holding a CannonBall.
        Item heldItem = playerInventory.GetItem();

        if (heldItem != null && heldItem.name == "CannonBall")
        {
            // GM: Destroy the cannonball object.
            Destroy(heldItem.gameObject);

            // GM: Clear the player's inventory.
            playerInventory.RemoveItem();

            Debug.Log("CannonBall loaded into the cannon and deleted!");
        }
        else
        {
            Debug.LogWarning("No CannonBall found in the player's inventory!");
        }
    }

    /// <summary>
    /// GM: Call this when the player interacts with the cannon.
    /// </summary>
    /// <param name="player">The player interacting with the cannon.</param>
    public void Interact()
    {
        //Task manager already does this - SD
        //Inventory playerInventory = player.GetComponent<Inventory>();

        //if (playerInventory != null)
        //{
        //    LoadCannon(playerInventory);
        //}
        //else
        //{
        //    Debug.LogWarning("Player does not have an Inventory component!");
        //}

        //Tell the kraken its been hit -SD
        kraken.krakenHit();
    }
}

