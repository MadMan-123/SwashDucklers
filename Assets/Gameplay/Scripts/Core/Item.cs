using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    
    public Inventory current;

    [SerializeField] public Vector3 offset;
    [SerializeField] public Quaternion pickupRotation;
    [SerializeField] public Type type;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;

    private void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!col) col = GetComponent<Collider>(); 
    }

    //type of item
    public void PickUp(GameObject source)
    {
        if (source.TryGetComponent(out Inventory inv) )       //try to see if player has an inv
        {
            current = inv;
            if (current.AddItem(gameObject))                              //add this to inv
            {
                /*//disable the rigidbody and collider
                rb.isKinematic = true;                           //bunch of positioning stuff
                col.enabled = false;
                
                //set the transforms
                transform.SetParent(current.itemHolder,true);

                transform.localPosition = offset;
                transform.localRotation = pickupRotation;*/
            }
            
        }
    }
    
    public void DropItem( )
    {

            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = false; 
            current.item = null;
            
            //set the inventory to null
            current = null;

            //set the transforms to allow the item to be dropped
            transform.SetParent(null);
    }
    
    
    
    //i think this would best be in Item instead of here, as so when refreing to types of item we can specify Item.Type.CannonBall, Item.Type.Plank, etc instead of ItemManager.Type.CannonBall, ItemManager.Type.Plank, etc - MW
    public enum Type
    {
        NoItem = -1,
        CannonBall,
        Plank,
        Cargo
        //other item types here
    }
}
