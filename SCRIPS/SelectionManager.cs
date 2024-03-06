using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    // Singleton instance of the SelectionManager
    public static SelectionManager Instance { get; set; }

    // Flag to determine if the cursor is on a valid target
    public bool onTarget;

    // The currently selected game object
    public GameObject selectedObject;

    // UI elements for interaction information
    public GameObject interaction_Info_UI;
    Text interaction_text;

    // Icons for indicating interaction
    public Image centerDotIcon;
    public Image handIcon;

    // Flag to determine if the hand is visible
    public bool handIsVisiblie;

    // The selected tree game object and its holder
    public GameObject selectedTree;
    public GameObject chopHolder;


    public GameObject selectedRock;
    public GameObject mineHolder;



    public GameObject selectedBuilding;
    public GameObject demolishHolder;

    // Start is called before the first frame update
    private void Start()
    {
        // Initialize variables
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Implement the Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Create a ray from the screen point forward into the scene
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the ray hits something
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            // Check if the hit object is a choppable tree
            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();
            if (choppableTree && choppableTree.playerInRange)
            {
                // Enable chopping for the tree and set the selected tree
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopHolder.gameObject.SetActive(true);
            }
            else
            {
                // If there was a previously selected tree, disable chopping
                if (selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
            }

            //Rock

            MineRock mineRock = selectionTransform.GetComponent<MineRock>();
            if (mineRock && mineRock.playerInRange)
            {
                
                mineRock.canBeMine = true;
                selectedRock = mineRock.gameObject;
                mineHolder.gameObject.SetActive(true);
            }
            else
            {
                
                if (selectedRock != null)
                {
                    selectedRock.gameObject.GetComponent<MineRock>().canBeMine = false;
                    selectedRock = null;
                    mineHolder.gameObject.SetActive(false);
                }
            }

            //Demolish foundation

            Demolition demolish = selectionTransform.GetComponent<Demolition>();
            if (demolish && demolish.playerInRange)
            {

                demolish.canBeDemolish = true;
                selectedBuilding = demolish.gameObject;
                demolishHolder.gameObject.SetActive(true);
            }
            else
            {

                if (selectedBuilding != null)
                {
                    selectedBuilding.gameObject.GetComponent<Demolition>().canBeDemolish = false;
                    selectedBuilding = null;
                    demolishHolder.gameObject.SetActive(false);
                }
            }




            // Check if the hit object is an interactable object
            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();
            if (interactable && interactable.playerInRange)
            {
                // Update onTarget flag and set the selected object
                onTarget = true;
                selectedObject = interactable.gameObject;

                // Update UI with interaction information
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);

                // Toggle icons based on the interactable's tag
                if (interactable.CompareTag("pickable"))
                {
                    centerDotIcon.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                    handIsVisiblie = true;
                }
                else
                {
                    centerDotIcon.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
                    handIsVisiblie = false;
                }
            }
            else // If there is a hit but without an Interactable Script.
            {
                onTarget = false;

                // Disable interaction UI elements
                interaction_Info_UI.SetActive(false);
                centerDotIcon.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
                handIsVisiblie = false;
            }
        }
        else // If there is no hit at all
        {
            onTarget = false;

            // Disable interaction UI elements
            interaction_Info_UI.SetActive(false);
            centerDotIcon.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);
            handIsVisiblie = false;
        }
    }

    // Method to disable the selection UI elements
    public void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotIcon.enabled = false;
        interaction_Info_UI.SetActive(false);

        selectedObject = null;
    }

    // Method to enable the selection UI elements
    public void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotIcon.enabled = true;
        interaction_Info_UI.SetActive(true);
    }
}
