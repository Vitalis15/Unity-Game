using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    //-----Player health---

    public float currentHealth;
    public float maxHealth;


    //-----Player Calorie----
    public float currentCalories;
    public float maxCalories;

    float distanceTravelled;
    Vector3 lastPosition;

    public GameObject playerBody;
    
    //----Player hydration----

    public float currentHydrationPercent;
    public float maxHydrationPercent;

    //public bool isHydrationActive;

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


    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth;

        currentCalories = maxCalories;

        currentHydrationPercent = maxHydrationPercent;

        StartCoroutine(deacreseHydration());

    }

    IEnumerator deacreseHydration()
    {
        while (true)
        {
            currentHydrationPercent -= 1;
            yield return new WaitForSeconds(5);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);

        lastPosition = playerBody.transform.position;

        if(distanceTravelled >= 10) 
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }




        // Test heaht
        if(Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10;
        }
    }

    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }
    public void setCalories(float newCalories)
    {
        currentCalories = newCalories;
    }
    
    public void setHydration (float newHydration)
    {
        currentHydrationPercent = newHydration;
    }
    

}
