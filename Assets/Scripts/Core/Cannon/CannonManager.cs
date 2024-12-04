using System.Collections.Generic;
using UnityEngine;

/// <summary>
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
    // GM: Reference to the player's inventory. This should be a script managing the player's items.
    public PlayerInventory playerInventory; 

    // GM: The cannon's inventory for storing loaded cannonballs.
    private List<GameObject> cannonInventory = new List<GameObject>();

    // GM: The maximum number of cannonballs the cannon can hold.
    [SerializeField] private int maxCannonCapacity = 1;

    // GM: Prefab for the cannonball. This will be instantiated or transferred.
    [SerializeField] private GameObject cannonBallPrefab;

    // GM: Check if the cannon is already loaded.
    public bool IsCannonLoaded => cannonInventory.Count > 0;

    /// <summary>
    /// GM: Attempts to load a cannonball into the cannon from the player's inventory.
    /// </summary>
    public void LoadCannon()
    {
        // GM: Check if the cannon is already loaded.
        if (IsCannonLoaded)
        {
            Debug.Log("Cannon is already loaded!");
            return;
        }

        // GM: Check if the player has a cannonball in their inventory.
        if (playerInventory.HasItem("CannonBall"))
        {
            // GM: Remove the cannonball from the player's inventory.
            GameObject cannonBall = playerInventory.RemoveItem("CannonBall");

            // GM: Add the cannonball to the cannon's inventory.
            cannonInventory.Add(cannonBall);

            Debug.Log("Cannonball loaded into the cannon!");
        }
        else
        {
            Debug.Log("Player does not have a cannonball to load!");
        }
    }

    /// <summary>
    /// GM: Fires the cannon and clears the inventory if loaded.
    /// </summary>
    public void FireCannon()
    {
        // GM: Check if the cannon is loaded.
        if (!IsCannonLoaded)
        {
            Debug.Log("Cannon is empty! Load it first.");
            return;
        }

        // GM: Fire the cannonball (implement your own firing logic).
        GameObject cannonBall = cannonInventory[0];
        cannonInventory.RemoveAt(0);

        Debug.Log("Cannon fired!");

        // GM: For now, destroy the fired cannonball (replace with your own logic).
        Destroy(cannonBall);
    }

    /// <summary>
    /// GM: Clears the cannon's inventory, if needed.
    /// </summary>
    public void ClearCannon()
    {
        cannonInventory.Clear();
        Debug.Log("Cannon inventory cleared.");
    }
}

/// <summary>
/// GM: Placeholder script for the player's inventory. Replace with your actual inventory system.
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    // GM: A dictionary to represent the player's inventory.
    private Dictionary<string, GameObject> inventory = new Dictionary<string, GameObject>();

    /// <summary>
    /// GM: Checks if the player has an item by name.
    /// </summary>
    public bool HasItem(string itemName)
    {
        return inventory.ContainsKey(itemName);
    }

    /// <summary>
    /// GM: Removes an item from the inventory and returns it.
    /// </summary>
    public GameObject RemoveItem(string itemName)
    {
        if (inventory.TryGetValue(itemName, out GameObject item))
        {
            inventory.Remove(itemName);
            return item;
        }
        return null;
    }

    /// <summary>
    /// GM: Adds an item to the inventory.
    /// </summary>
    public void AddItem(string itemName, GameObject item)
    {
        if (!inventory.ContainsKey(itemName))
        {
            inventory[itemName] = item;
        }
    }
}
