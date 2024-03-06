using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demolition : MonoBehaviour
{

    public bool playerInRange;
    public bool canBeDemolish;

    public float FoundationkMaxHealth;
    public float FoundationHealth;

    public float caloriesSpentDemolish = 10;


    // Start is called before the first frame update
    void Start()
    {
        FoundationHealth = FoundationkMaxHealth;
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

        FoundationHealth -= 1;
        PlayerState.Instance.currentCalories -= caloriesSpentDemolish;

        // Check if the rock is dead
        if (FoundationHealth <= 0)
        {
            Demolish();
           // SoundManager.Instance.PlaySound(SoundManager.Instance.breakingRock);
        }
    }

    void Demolish()
    {
        Vector3 rockPosition = transform.position;
        Destroy(transform.gameObject);
        canBeDemolish = false;

        SelectionManager.Instance.selectedBuilding = null;
        SelectionManager.Instance.demolishHolder.gameObject.SetActive(false);

       // GameObject brokenRock = Instantiate(Resources.Load<GameObject>("MineRock"),
        //    new Vector3(rockPosition.x, rockPosition.y + 2.5f, rockPosition.z), Quaternion.Euler(0, 0, 0));
    }


    // Update is called once per frame
    void Update()
    {
        // Update global state variables if the rock can be mine
        if (canBeDemolish)
        {
            GlobalState.Instance.resourceHealth = FoundationHealth;
            GlobalState.Instance.resourceMaxHealth = FoundationkMaxHealth;
        }



    }
}
