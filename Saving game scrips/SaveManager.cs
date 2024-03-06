using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;


using System.IO;
using static ConstructionData;

public class SaveManager : MonoBehaviour
{

    public static SaveManager Instance { get; set; }


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



    //Json Project Save Path
    string jsonPathProject;

    //Json External/Real Save Path
    string jsonPathPersistante;

    //Binary Save Path
    string binaryPath;

    string fileName = "SaveGame";

    public bool isSavingToJson;

    public bool isLoading;

    public Canvas loadingScreen;

    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
        jsonPathPersistante = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }

    #region || ------ General Section ----- ||

    #region || ------ Saving ----- ||

    public void SaveGame(int slotNumber)
    {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();

        data.enviromentData = GetEnviromentData();
        data.constructionData = GetConstructionData();

        SavingTypeSwitch(data, slotNumber);
    }

    private ConstructionData GetConstructionData()
    {
        List<ConstructionItemData> constructedItems = new List<ConstructionItemData>();

        foreach (GameObject construction in ConstructionManager.Instance.allGhostsInExistence)
        {
            // Gather relevant information about each construction item
            string name = construction.name;
            Vector3 position = construction.transform.position;
            Quaternion rotation = construction.transform.rotation;

            ConstructionItemData itemData = new ConstructionItemData(name, position, rotation);
            constructedItems.Add(itemData);
        }

        return new ConstructionData(constructedItems);
    }


    private EnviromentData GetEnviromentData()
    {
        List<string> itemPickup = InventorySystem.Instance.itemPickedup;

        return new EnviromentData(itemPickup);
    }

    private PlayerData GetPlayerData()
    {
        float[] playerStats = new float[3];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentCalories;
        playerStats[2] = PlayerState.Instance.currentHydrationPercent;

        float[] playerPosAndRot = new float[6];

        playerPosAndRot[0] = PlayerState.Instance.playerBody.transform.position.x;
        playerPosAndRot[1] = PlayerState.Instance.playerBody.transform.position.y;
        playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.position.z;

        playerPosAndRot[3] = PlayerState.Instance.playerBody.transform.rotation.x;
        playerPosAndRot[4] = PlayerState.Instance.playerBody.transform.rotation.y;
        playerPosAndRot[5] = PlayerState.Instance.playerBody.transform.rotation.z;

        string[] inventory = InventorySystem.Instance.itemList.ToArray();
        string[] quickSlots = GetQuickSlotsContent();

        return new PlayerData(playerStats, playerPosAndRot, inventory, quickSlots);

    }

    private string[] GetQuickSlotsContent()
    {
        List<string> temp = new List<string>();

        foreach (GameObject slot in EquipSystem.Instance.quickSlotsList)
        {
            if(slot.transform.childCount != 0)
            {
                string name = slot.transform.GetChild(0).name;
               // string str2 = "(Clone)";
                string cleanName = name.Replace("(Clone)", "");
                temp.Add(cleanName);
            }
        }
        return temp.ToArray();
    }

    public bool SavingTypeSwitch(AllGameData gameData, int slotNumber)
    {
        if (isSavingToJson)
        {
            SaveGameToJsonFile(gameData, slotNumber);
        }
        else
        {
            SaveGameToBinaryFile(gameData, slotNumber);
        }
        return true;
    }
    #endregion

    #region || ------ Loading ----- ||

    public AllGameData LoadingTypeSwitch(int slotNumber)
    {
        if (isSavingToJson)
        {
            AllGameData gameData = LoadGameToJsonFile(slotNumber);
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGameToBinaryFile(slotNumber);
            return gameData;
        }
    }

    public void LoadGame(int slotNumber)
    {
        // Player Data
        SetPlayerData(LoadingTypeSwitch(slotNumber).playerData);

        //Enviroment Data
        SetEnviromentData(LoadingTypeSwitch(slotNumber).enviromentData);
        // Construction Data
        SetConstructionData(LoadingTypeSwitch(slotNumber).constructionData);

        isLoading = false;

        DisableLoadingScreen();
    }

    private void SetConstructionData(ConstructionData constructionData)
    {
        foreach (ConstructionItemData itemData in constructionData.constructedItems)
        {
            // Adjust the following instantiation based on your construction item prefab
            GameObject constructedItem = Instantiate(Resources.Load<GameObject>(itemData.itemName));
            constructedItem.transform.position = itemData.position;
            constructedItem.transform.rotation = itemData.rotation;

            ConstructionManager.Instance.allGhostsInExistence.Add(constructedItem);
        }
    }

    private void SetEnviromentData(EnviromentData enviromentData)
    {
        foreach (Transform itemType in EnviromenManager.Instance.allItems.transform)
        {
            foreach(Transform item in itemType.transform)
            {
                if (enviromentData.pickedupItems.Contains(item.name))
                {
                    Destroy(item.gameObject);
                }
            }
        }

        InventorySystem.Instance.itemPickedup = enviromentData.pickedupItems;
    }

    

    private void SetPlayerData(PlayerData playerData)
    {
        //Setting Player Stats
        PlayerState.Instance.currentHealth = playerData.playerStats[0];
        PlayerState.Instance.currentCalories = playerData.playerStats[1];
        PlayerState.Instance.currentHydrationPercent = playerData.playerStats[2];

        //Setting Player Position

        Vector3 loadedPosition;
        loadedPosition.x = playerData.playerPositionAndRotation[0];
        loadedPosition.y = playerData.playerPositionAndRotation[1];
        loadedPosition.z = playerData.playerPositionAndRotation[2];

        PlayerState.Instance.playerBody.transform.position = loadedPosition;

        // Setting Player Rotation

        Vector3 loadedRotation;
        loadedRotation.x = playerData.playerPositionAndRotation[3];
        loadedRotation.y = playerData.playerPositionAndRotation[4];
        loadedRotation.z = playerData.playerPositionAndRotation[5];

    
        PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);
        //Setting the inventory content

        foreach (string item in playerData.inventoryContent)
        {
            InventorySystem.Instance.AddToInventory(item);
        }
        //Setting the quick slots content
        foreach (string item in playerData.quickSlotsContent)
        {
            //Find next free quick slot
            GameObject availableSlot = EquipSystem.Instance.FindNextEmptySlot();
            var itemToAdd = Instantiate(Resources.Load<GameObject>(item));

            itemToAdd.transform.SetParent(availableSlot.transform, false);
        }

       
    }

    public void StartLoadedGame(int slotNumber)  // old name LoadSaveGame
    {
        ActivateLoadingScreen();


        isLoading = true;
        SceneManager.LoadScene("GameScene");
        StartCoroutine(DelayedLoading(slotNumber));
    }

    private IEnumerator DelayedLoading(int slotNumber)
    {
        yield return new WaitForSeconds(1f);
        LoadGame(slotNumber);

        print("Game Loaded");
    }

    #endregion

    #endregion

    #region || ------ To Binary Section ----- ||

    public void SaveGameToBinaryFile(AllGameData gameDate, int slotNumber)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        
        FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Create);

        formatter.Serialize(stream, gameDate);
        stream.Close();

        print("Data save to" + binaryPath + fileName + slotNumber + ".bin");

    }
    
    public AllGameData LoadGameToBinaryFile(int slotNumber)
    {
        
        if (File.Exists(binaryPath + fileName + slotNumber + ".bin"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();

            print("Data loaded from" + binaryPath + fileName + slotNumber + ".bin");



            return data;

        }
        else
        {
            return null;
        }

    }


    #endregion

    #region || ------ To Json Section ----- ||

    public void SaveGameToJsonFile(AllGameData gameDate, int slotNumber)
    {
        string json = JsonUtility.ToJson(gameDate);

        //string encryted = EncryptionDecryption(json);

        using (StreamWriter write = new StreamWriter(jsonPathProject + fileName + slotNumber + ".json"))
        {
            write.Write(json);//ecryption
            print("Saved Game to Json file at:" + jsonPathProject + fileName + slotNumber + ".json");
        };
     

    }

    public AllGameData LoadGameToJsonFile(int slotNumber)
    {
        using (StreamReader reader = new StreamReader(jsonPathProject + fileName + slotNumber + ".json"))
        {
            string json = reader.ReadToEnd();

           //string decrypted = EncryptionDecryption(json);

            AllGameData data = JsonUtility.FromJson<AllGameData>(json); // encryption
            return data;
        };

    }

    #endregion

    #region || ------ Settings Section -------  ||

    #region || ------ Volume Settings -------  ||

    [System.Serializable]
    public class VolumeSettings
    {
        public float music;
        public float effects;
        public float master;
    }

    public void SaveVolumeSettings(float _music, float _effects, float _master)
    {
        VolumeSettings volumeSettings = new VolumeSettings()
        {
            music = _music,
            effects = _effects,
            master = _master
        };
        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings));
        PlayerPrefs.Save();

        print("Saved to Player Pref");
    }
  
    public VolumeSettings LoadVolumeSettings()
    {
        return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));       
        
    }


    public float LoadMusicVolume()
    {
        var volumeSetting = JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
        return volumeSetting.music;


    }
    #endregion





    #endregion


    #region || ----- Encryption ----- ||

    public string EncryptionDecryption(string jsonString)
    {
        string keyword = "1234567";
        string result = "";

        for (int i =0; i < jsonString.Length; i++)
        {
            result += (char)(jsonString[i] ^ keyword[i % keyword.Length]);
        }
        return result;
    }




    #endregion
    #region || ------ LoadingScreen Section ----- ||

    public void ActivateLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //music for loading screen

        //animation

        //show tips

    }
    public void DisableLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(false);
    }

    #endregion

    #region || ----- Utility ----- ||

    public bool DoesFileExists(int slotNumber)
    {
        if(isSavingToJson)
        {
            if (System.IO.File.Exists(jsonPathProject + fileName + slotNumber + ".json")) //SaveGame1.json
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            if (System.IO.File.Exists(binaryPath + fileName + slotNumber + ".bin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool isSlotEmpty(int slotNumber)
    {
        if (DoesFileExists(slotNumber))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void DeselectButton()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }
    #endregion

}
