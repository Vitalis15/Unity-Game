using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{

    public bool playerInRange;

    public string ItemName;

    public string GetItemName()
    {
        return ItemName;
    }

    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)&&playerInRange&&SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            //if the inentori is not full
            if (InventorySystem.Instance.CheckSlotsAvailable(1)) 
            {
                InventorySystem.Instance.AddToInventory(ItemName);

                InventorySystem.Instance.itemPickedup.Add(gameObject.name);

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("inventory is full");
            }
            

        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;


        }
    
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        
        }

    }

}
