using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    // Reference to the UI element displaying item information
    public GameObject ItemInfoUi;

    public static InventorySystem Instance { get; set; }      // Singleton pattern to provide a single instance of the inventory system

    public GameObject inventoryScreenUI;      // Reference to the main inventory UI

    public List<GameObject> slotList = new List<GameObject>();  // List to store references to all inventory slots

    public List<string> itemList = new List<string>();  // List to store the names of items in the inventory

    // Variables related to the pickup alert popup
    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    // Boolean flag to track if the inventory screen is open

    public bool isOpen;


    //PickUp Popup // Reference to the pickup alert UI elements
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;


    public List<string> itemPickedup;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start() // Start is called before the first frame update
    {
        isOpen = false;    
        //isFull = false;
        PopulateSlotList();    // Populate the slot list with references to slots in the inventory UI


        Cursor.visible = false;


    }
    // Populates the slotList with references to slot GameObjects in the inventory UI
    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()     // Update is called once per frame
    {
        if (Input.GetKeyDown(KeyCode.I) && !isOpen && !ConstructionManager.Instance.inConstructionMode)  // Toggle the inventory screen on/off when the 'I' key is pressed
        {
            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            // If the crafting screen is not open, set the cursor to be locked and invisible
            if (!CraftingSystem.Instance.isOpen) 
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                // Enable selection manager when closing the inventory
                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }

            isOpen = false;
        }
    }

    public void AddToInventory(string ItemName)     // Adds an item to the inventory
    {
        if (SaveManager.Instance.isLoading == false)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.pickItemSound);
        }
        else
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.pickItemSound);
        }
        
        

            // Find the next empty slot in the inventory
            whatSlotToEquip = FindNextEmptySlot();
            // Instantiate the item and place it in the empty slot
            itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(ItemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);
            // Add the item name to the itemList
            itemList.Add(ItemName);
            // Trigger the pickup alert popup
            TriggerPickupPopup(ItemName, itemToAdd.GetComponent<Image>().sprite);

            // Recalculate the inventory list and refresh needed items for crafting
            ReCalculateList();
            CraftingSystem.Instance.RefreshNeededItems();
        
    
    }
    // Displays a popup alert for the picked-up item
    void TriggerPickupPopup(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;

        // Start the coroutine to hide the pickup alert after 3 seconds
        StartCoroutine(HidePickupPopupAfterDelay(3f));

    }

    // Coroutine to hide the pickup alert after a delay
    private IEnumerator HidePickupPopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Code to hide the pickup alert
        pickupAlert.SetActive(false);
    }




    // Finds the next empty slot in the inventory
    private GameObject FindNextEmptySlot()
    {
       foreach(GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0) 
            { 
                return slot; 
            }
        }
        return new GameObject();

    }

    // Checks if a specified number of slots is available in the inventory
    public bool CheckSlotsAvailable(int emptyMeeded)
    {
        int emptySlot = 32;

        foreach (GameObject slot in slotList) 
        {
            if(slot.transform.childCount > 0) 
            {
                emptySlot -= 1; 
            }
        }
        Debug.Log("Empty Slots: " + emptySlot);
        if (emptySlot >= emptyMeeded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // Removes items from the inventory based on the specified name and amount
    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for (var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);

                    counter -= 1;

                }
            }
        }
        // Recalculate the inventory list and refresh needed items for crafting
        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }


    // Recalculates the itemList based on the items in the inventory slots
    public void ReCalculateList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                // Extract the item name from the cloned item's name
                string name = slot.transform.GetChild(0).name;
                //string str1 = name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");

                itemList.Add(result);
            }
        }
    }

}