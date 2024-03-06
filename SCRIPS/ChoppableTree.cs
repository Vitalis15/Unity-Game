using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
{
    // Flags to track player proximity and whether the tree can be chopped
    public bool playerInRange;
    public bool canBeChopped;

    // Tree health variables
    public float treeMaxHealth;
    public float treeHealth;

    // Animator for tree animations
    public Animator animator;

    // Calories spent on chopping wood
    public float caloriesSpentChoppingWood = 10;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize tree health and get the animator from the parent object
        treeHealth = treeMaxHealth;
        animator = transform.parent.transform.parent.GetComponent<Animator>();
    }

    // Called when another collider enters the trigger zone
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is in range for interaction
            playerInRange = true;
        }
    }

    // Called when another collider exits the trigger zone
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player is no longer in range
            playerInRange = false;
        }
    }

    // Method to handle tree being hit
    public void GetHit()
    {
        // Trigger shake animation for the tree
        animator.SetTrigger("shake");

        // Reduce tree health and deduct calories from the player
        treeHealth -= 2;
        PlayerState.Instance.currentCalories -= caloriesSpentChoppingWood;

        // Check if the tree is dead
        if (treeHealth <= 0)
        {
            treeIsDead();
            SoundManager.Instance.PlaySound(SoundManager.Instance.falingTree);
        }
    }

    // Method to handle the tree being dead
    void treeIsDead()
    {
        // Get the position of the tree
        Vector3 treePosition = transform.position;

        // Destroy the tree object
        Destroy(transform.parent.transform.parent.gameObject);

        // Disable chopping for the tree and reset selected tree in the SelectionManager
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        // Instantiate a broken tree object at the same position
        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("ChoppedTree"),
            new Vector3(treePosition.x, treePosition.y + 3.3f, treePosition.z), Quaternion.Euler(0, 0, 0));
    }

    // Update is called once per frame
    private void Update()
    {
        // Update global state variables if the tree can be chopped
        if (canBeChopped)
        {
            GlobalState.Instance.resourceHealth = treeHealth;
            GlobalState.Instance.resourceMaxHealth = treeMaxHealth;
        }
    }
}
