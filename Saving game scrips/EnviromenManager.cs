using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromenManager : MonoBehaviour
{
    public static EnviromenManager Instance { get; set; }

    public GameObject allItems;


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
        DontDestroyOnLoad(gameObject);
    }



}
