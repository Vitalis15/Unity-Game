using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    // Reference to the Animator component
    public Animator animator;

    // Flag to control the delay between swings
    public bool swingWait;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check conditions for initiating a swing
        if (Input.GetMouseButtonDown(0) &&
            !InventorySystem.Instance.isOpen &&           // Check if the inventory is not open
            !CraftingSystem.Instance.isOpen &&           // Check if the crafting system is not open
            !SelectionManager.Instance.handIsVisiblie &&  // Check if the hand is not visible
            !swingWait &&                                // Check if not currently waiting for the swing
            !ConstructionManager.Instance.inConstructionMode) // Check if not in construction mode
        {
            // Set swingWait to true to prevent rapid swings
            swingWait = true;

            // Initiate the swing animation and sound
            StartCoroutine(SwingSoundDelay());
            animator.SetTrigger("hit");

            // Wait for a delay before allowing the next swing
            StartCoroutine(NewSwingDelay());
        }
    }

    // Method to handle the "hit" action
    public void GetHit()
    {
        // Get the selected tree from the SelectionManager
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        // Check if a tree is selected
        if (selectedTree != null)
        {
            // Call the GetHit method on the ChoppableTree component of the selected tree
            selectedTree.GetComponent<ChoppableTree>().GetHit();

            // Play the chopping sound
            SoundManager.Instance.PlaySound(SoundManager.Instance.chopSound);
        }

      

    }
    public void GetHit1()//pickaxe hit action
    {
        GameObject selectedRock = SelectionManager.Instance.selectedRock;
        if (selectedRock != null)
        {
            selectedRock.GetComponent<MineRock>().GetHit();
            SoundManager.Instance.PlaySound(SoundManager.Instance.pickAxeHit);//pickaxe hit sound
        }


    }

    public void GetDemolish()
    {
        GameObject selectedBuilding = SelectionManager.Instance.selectedBuilding;
        if (selectedBuilding != null)
        {
            selectedBuilding.GetComponent<Demolition>().GetHit();
           
        }
    }
    // Coroutine for introducing a delay before playing the swing sound
    IEnumerator SwingSoundDelay()
    {
        // Wait for 0.2 seconds
        yield return new WaitForSeconds(0.2f);

        // Play the tool swing sound
        SoundManager.Instance.PlaySound(SoundManager.Instance.toolSwingSound);
    }

    // Coroutine for introducing a delay before allowing the next swing
    IEnumerator NewSwingDelay()
    {
        // Wait for 1.2 seconds
        yield return new WaitForSeconds(1.2f);

        // Set swingWait to false to allow the next swing
        swingWait = false;
    }
}
