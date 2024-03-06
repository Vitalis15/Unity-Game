using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    // References to UI elements
    public GameObject craftingScreenUI;
    public GameObject toolsScreenUI, survivalScreenUI, refineScreenUI, constructionScreenUI;
    // List to keep track of the player's inventory
    public List<string> inventoryItemList = new List<string>();

    // Buttons for different crafting categories

    Button toolsBTN, survivalBTN, refineBTN, constructionBTN;

    //Craf BUTTONS

    Button craftAxeBTN; // AxeButton
    Button craftPickaxeBTN; // Pickaxe Button
    Button craftSpearBTN; // Spear button

    Button craftPlankBTN;// plank button
    Button craftStickBTN;// stick button

    Button craftFoundationBTN;
    Button craftWallBTN;
   

    //Requirement Text

    Text AxeReq1, AxeReq2; // Axe
    Text PickaxeReq1, PickaxeReq2;//Pickaxe
    Text SpearReq1, SpearReq2; //Spear

    Text FoundationReq1;
    Text WallReq1;
   

    Text PlankReq1; // plank
    Text StickReq1; //Stick 

    



    public bool isOpen;

    // Blueprint instances for different crafted items //
    // { ("name_of_item" , "how_many_shoud_craft" , "how_many_componets",  "Name_of_comp", "num_of_comp", "Name_of_comp", "num_of_comp", "Name_of_comp", "num_of_comp",) 
    public BluePrint AXEBLP = new BluePrint("Axe", 1 ,2,"Flint",1,"Branch", 1,"",0);
    public BluePrint PICKAXEBLP = new BluePrint("Pickaxe", 1, 2, "Flint", 2, "Stick", 1, "", 0);
    public BluePrint SPEARBLP = new BluePrint("Spear", 1, 2, "Flint", 1, "Stick", 2, "", 0);

    public BluePrint PLANKBLP = new BluePrint("Plank", 2, 1, "Log", 1,"",0, "", 0);
    public BluePrint STICKBLP = new BluePrint("Stick", 2, 1, "Branch", 1, "", 0, "", 0);

    public BluePrint FOUNDATIONBLP = new BluePrint("Foundation", 1, 1, "Plank", 3, "", 0, "", 0);
    public BluePrint WALLBLP = new BluePrint("Wall", 1, 1, "Plank", 3, "", 0, "", 0);




    // Singleton instance of the CraftingSystem
    public static CraftingSystem Instance {  get; set; }

    private void Awake()
    {    // Ensure only one instance of CraftingSystem exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else 
        { 
            Instance = this; 
        }
    }





    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        // Assigning event listeners to category buttons
        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolCategory(); });

        Debug.Log("Tools Button: " + (toolsScreenUI.transform.Find("ToolsButton") != null));

        survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBTN.onClick.AddListener(delegate { OpenSurvivaCategory(); });

        Debug.Log("refineScreenUI Button: " + (refineScreenUI.transform.Find("RefineButton") != null));

        refineBTN = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
        refineBTN.onClick.AddListener(delegate { OpenRefineCategory(); });

        constructionBTN = craftingScreenUI.transform.Find("ConstructionButton").GetComponent<Button>();
        constructionBTN.onClick.AddListener(delegate { OpenConstructionCategory(); });



        //Axe
        AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>();
        AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("Button").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AXEBLP); });

        //Pickaxe
        PickaxeReq1 = toolsScreenUI.transform.Find("Pickaxe").transform.Find("req1").GetComponent<Text>();
        PickaxeReq2 = toolsScreenUI.transform.Find("Pickaxe").transform.Find("req2").GetComponent<Text>();

        craftPickaxeBTN = toolsScreenUI.transform.Find("Pickaxe").transform.Find("Button").GetComponent<Button>();
        craftPickaxeBTN.onClick.AddListener(delegate { CraftAnyItem(PICKAXEBLP); });

        //Spear
        SpearReq1 = toolsScreenUI.transform.Find("Spear").transform.Find("req1").GetComponent<Text>();
        SpearReq2 = toolsScreenUI.transform.Find("Spear").transform.Find("req2").GetComponent<Text>();

        craftSpearBTN = toolsScreenUI.transform.Find("Spear").transform.Find("Button").GetComponent<Button>();
        craftSpearBTN.onClick.AddListener(delegate { CraftAnyItem(SPEARBLP); });


        //Plank
        PlankReq1 = refineScreenUI.transform.Find("Plank").transform.Find("req1").GetComponent<Text>();


        craftPlankBTN = refineScreenUI.transform.Find("Plank").transform.Find("Button").GetComponent<Button>();
        craftPlankBTN.onClick.AddListener(delegate { CraftAnyItem(PLANKBLP); });

        //Stick
        StickReq1 = refineScreenUI.transform.Find("Stick").transform.Find("req1").GetComponent<Text>();

        craftStickBTN = refineScreenUI.transform.Find("Stick").transform.Find("Button").GetComponent<Button>();
        craftStickBTN.onClick.AddListener(delegate { CraftAnyItem(STICKBLP); });

        //Foundation
        FoundationReq1 = constructionScreenUI.transform.Find("Foundation").transform.Find("req1").GetComponent<Text>();

        craftFoundationBTN = constructionScreenUI.transform.Find("Foundation").transform.Find("Button").GetComponent<Button>();
        craftFoundationBTN.onClick.AddListener(delegate { CraftAnyItem(FOUNDATIONBLP); });

        //Wall
        WallReq1 = constructionScreenUI.transform.Find("Wall").transform.Find("req1").GetComponent<Text>();

        craftWallBTN = constructionScreenUI.transform.Find("Wall").transform.Find("Button").GetComponent<Button>();
        craftWallBTN.onClick.AddListener(delegate { CraftAnyItem(WALLBLP); });




    }


    void OpenToolCategory()   // Activate Tools crafting screen and deactivate others
    {
        craftingScreenUI.SetActive(false);

        toolsScreenUI.SetActive(true);

        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
    }


    void OpenSurvivaCategory()    // Activate Survival crafting screen and deactivate others
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);

        survivalScreenUI.SetActive(true);

        refineScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);

    }

    void OpenRefineCategory() // Activate Refine crafting screen and deactivate others
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(true);

        constructionScreenUI.SetActive(false);
    }

    void OpenConstructionCategory()
    {
        craftingScreenUI.SetActive(false);
        toolsScreenUI.SetActive(false);
        survivalScreenUI.SetActive(false);
        refineScreenUI.SetActive(false);

        constructionScreenUI.SetActive(true);
    }

    void CraftAnyItem(BluePrint blueprintToCraft)  // Function to craft an item based on the provided blueprint
    {

        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound); // sound for crafting
        //produce the amount of items accordint to the blueprint
        for (var i = 0; i<blueprintToCraft.numOfItemsToProduce; i++)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.ItemName);           

        }




        // Check the required items in the inventory and remove them

        if (blueprintToCraft.numOfRequirements ==1) 
        { 
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount); 
        }
        else if (blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
        }
        else if (blueprintToCraft.numOfRequirements == 3)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.Req3, blueprintToCraft.Req3amount);
        }
        // Refresh inventory list after a delay
        StartCoroutine(calculate());

    }
    // Coroutine to delay the recalculation of the inventory list
    public IEnumerator calculate()
    {
        yield return new WaitForSeconds(1f);
        InventorySystem.Instance.ReCalculateList();
        RefreshNeededItems();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.C) && !isOpen && !ConstructionManager.Instance.inConstructionMode)   // Toggle crafting screen with 'C' key
        {
            Debug.Log("c is pressed");
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);
            refineScreenUI.SetActive(false);
            constructionScreenUI.SetActive(false);

            // Enable cursor and selection manager when crafting screen is closed
            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;


                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }

            isOpen = false;
        }
    }


    // Method to refresh the required items in the crafting UI
    public void RefreshNeededItems()

    {   // Initialize counts for different resources
        int stone_count = 0;
        int stick_count = 0;
        int flint_count = 0;
        int branch_count = 0;
        int log_count = 0;
        int plank_count = 0;


        inventoryItemList = InventorySystem.Instance.itemList;      // Get the current inventory items

        foreach (string ItemName in inventoryItemList)       // Count the occurrences of each resource
        {
            switch(ItemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;

                case "Flint":
                    flint_count += 1;
                    break;

                case "Branch":
                    branch_count += 1;
                    break;

                case "Log":
                    log_count += 1;
                    break;
                case "Plank":
                    plank_count += 1;
                    break;
            }
        }

        //-----AXE----//      // Update requirement texts and button visibility for Axe

        AxeReq1.text = "1 Branch [" + branch_count + "]";
        AxeReq2.text = "1 Flint [" + flint_count + "]";

        if (flint_count >= 1 && branch_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false); 
        }

        //-----PICKAXE----//          // Update requirement texts and button visibility for Pickaxe

        PickaxeReq1.text = "1 Stick [" + stick_count + "]";
        PickaxeReq2.text = "2 Flint [" + flint_count + "]";

        if (flint_count >= 2 && stick_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftPickaxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftPickaxeBTN.gameObject.SetActive(false);
        }

        //-----Spear----//     // Update requirement texts and button visibility for Spear

        SpearReq1.text = "2 Stick [" + stick_count + "]";
        SpearReq2.text = "1 Flint [" + flint_count + "]";

        if (flint_count >= 1 && stick_count >= 2 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftSpearBTN.gameObject.SetActive(true);
        }
        else
        {
            craftSpearBTN.gameObject.SetActive(false);
        }


        //-----Plank----// // Update requirement texts and button visibility for Plank

        PlankReq1.text = "1 Log [" + log_count + "]";
        

        if (log_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(2))
        {
            craftPlankBTN.gameObject.SetActive(true);
        }
        else
        {
            craftPlankBTN.gameObject.SetActive(false);
        }


        //-----Stick----//
        StickReq1.text = "1 Branch [" + branch_count + "]";


        if (branch_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(1))
        {
            craftStickBTN.gameObject.SetActive(true);
        }
        else
        {
            craftStickBTN.gameObject.SetActive(false);
        }


        //-----Foundation----//
        FoundationReq1.text = "3 Plank [" + plank_count + "]";


        if (plank_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1)) // Remenber to change the number
        {
            craftFoundationBTN.gameObject.SetActive(true);
        }
        else
        {
            craftFoundationBTN.gameObject.SetActive(false);
        }

        //-----Wall----//
        WallReq1.text = "3 Plank [" + plank_count + "]";


        if (plank_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))// Remenber to change the number
        {
            craftWallBTN.gameObject.SetActive(true);
        }
        else
        {
            craftWallBTN.gameObject.SetActive(false);
        }





    }






}
