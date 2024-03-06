using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    // Singleton instance
    public static SettingsManager Instance { get; set; }

    // UI elements for settings
    public Button backBTN;

    public Slider masterSlider;
    public GameObject masterValue;

    public Slider musicSlider;
    public GameObject musicValue;

    public Slider effectsSlider;
    public GameObject effectsValue;

    // Executed when the script starts
    private void Start()
    {
        // Attach a listener to the back button to save volume settings when clicked
        backBTN.onClick.AddListener(() =>
        {
            SaveManager.Instance.SaveVolumeSettings(musicSlider.value, effectsSlider.value, masterSlider.value);
        });

        // Start a coroutine to load and apply settings with a slight delay
        StartCoroutine(LoadAndApplySetting());
    }

    // Coroutine to load and apply settings with a slight delay
    private IEnumerator LoadAndApplySetting()
    {
        // Load and set volume settings
        LoadAndSetVolume();

        // Wait for a short time (0.1 seconds) to allow for initialization
        yield return new WaitForSeconds(0.1f);
    }

    // Load and set volume settings from saved data
    private void LoadAndSetVolume()
    {
        // Load volume settings from SaveManager
        SaveManager.VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();

        // Apply loaded volume settings to sliders
        masterSlider.value = volumeSettings.master;
        musicSlider.value = volumeSettings.music;
        effectsSlider.value = volumeSettings.effects;

        // Print a message indicating that volume settings are loaded
        print("Volume Setting are loaded");
    }

    // Executed when the script is awakened
    private void Awake()
    {
        // Ensure only one instance of SettingsManager exists, destroy duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Executed every frame
    private void Update()
    {
        // Update UI text elements to display current slider values
        masterValue.GetComponent<TextMeshProUGUI>().text = "" + (masterSlider.value) + "";
        musicValue.GetComponent<TextMeshProUGUI>().text = "" + (musicSlider.value) + "";
        effectsValue.GetComponent<TextMeshProUGUI>().text = "" + (effectsSlider.value) + "";
    }
}
