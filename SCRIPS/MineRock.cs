using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineRock : MonoBehaviour
{

    public bool playerInRange;
    public bool canBeMine;

    public float RockMaxHealth;
    public float RockHealth;

    public float caloriesSpentMiningRock = 10;
    void Start()
    {
        RockHealth = RockMaxHealth;
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


    public void GetHit()
    {

        RockHealth -= 1;
        PlayerState.Instance.currentCalories -= caloriesSpentMiningRock;

        // Check if the rock is dead
        if (RockHealth <= 0)
        {
            rockIsDead();
            SoundManager.Instance.PlaySound(SoundManager.Instance.breakingRock);
        }
    }

    void rockIsDead()
    {
        Vector3 rockPosition = transform.position;
        Destroy(transform.parent.gameObject);
        canBeMine = false;

        SelectionManager.Instance.selectedRock = null;
        SelectionManager.Instance.mineHolder.gameObject.SetActive(false);

        GameObject brokenRock = Instantiate(Resources.Load<GameObject>("MineRock"),
            new Vector3(rockPosition.x, rockPosition.y + 2.5f, rockPosition.z), Quaternion.Euler(0, 0, 0));
    }
    private void Update()
    {

        // Update global state variables if the rock can be mine
        if (canBeMine)
        {
            GlobalState.Instance.resourceHealth = RockHealth;
            GlobalState.Instance.resourceMaxHealth = RockMaxHealth;
        }
    }
    
}
