using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static MenuManager Instance { get; set; }

    public GameObject menuCanvas;
    public GameObject uiCanvas;

    public bool isMenuOpen;

    public GameObject saveMenu;
    public GameObject settingMenu;
    public GameObject menu;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M) && !isMenuOpen)  //(KeyCode.Escape)
        {
            uiCanvas.SetActive(false);
            menuCanvas.SetActive(true);

            isMenuOpen = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
        }
        else if(Input.GetKeyDown(KeyCode.M) && isMenuOpen)
        {
            saveMenu.SetActive(false);
            settingMenu.SetActive(false);
            menu.SetActive(true);


            uiCanvas.SetActive(true);
            menuCanvas.SetActive(false);

            isMenuOpen = false;


            if (CraftingSystem.Instance.isOpen == false && InventorySystem.Instance.isOpen == false) 
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = true;
            }
           


            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
        }
    }

   

}
