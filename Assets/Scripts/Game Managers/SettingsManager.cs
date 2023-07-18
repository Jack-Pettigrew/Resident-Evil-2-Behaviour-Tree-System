using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Graphics Settings Fields")]
    [SerializeField] private TMP_Dropdown fullscreenMode;
    [SerializeField] private TMP_Dropdown resolution;
    [SerializeField] private TMP_Dropdown quality;
    [SerializeField] private Toggle vsync;

    [Header("Audio Settings Fields")]
    [SerializeField] private Slider masterAudioSlider;
    [SerializeField] private DD.UI.SliderValueIndicator masterAudioSliderValue;

    public void RefreshFields()
    {
        LoadSettingsFields();
        ApplySavedSettings();
    }
    
    /// <summary>
    /// Clears settings values from PlayerPrefs and sets the defaults.
    /// </summary>
    [ContextMenu("Reset PlayerPrefs")]
    public void ResetSettings()
    {
        PlayerPrefs.DeleteAll();

        SaveSettings();
        ApplySavedSettings();
        LoadSettingsFields();
    }
    
    /// <summary>
    /// Applies the settings currently 
    /// </summary>
    public void ApplySelectedSettings()
    {
        Screen.SetResolution(Screen.resolutions[resolution.value].width, Screen.resolutions[resolution.value].height, (FullScreenMode)fullscreenMode.value);
        QualitySettings.SetQualityLevel(quality.value, true);
        QualitySettings.vSyncCount = vsync.isOn ? 1 : 0;

        AudioListener.volume = masterAudioSlider.value;
        
        SaveSettings();
    }

    /// <summary>
    /// Applies any settings values saved in PlayerPrefs to the Unity Player.
    /// </summary>
    public void ApplySavedSettings()
    {
        if(PlayerPrefs.HasKey("resolution"))
        {
            Screen.SetResolution(Screen.resolutions[PlayerPrefs.GetInt("resolution")].width, Screen.resolutions[PlayerPrefs.GetInt("resolution")].height, (FullScreenMode)PlayerPrefs.GetInt("fullscreen_mode"));
        }

        if(PlayerPrefs.HasKey("graphics_quality"))
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("graphics_quality"), true);
        }

        if(PlayerPrefs.HasKey("vsync"))
        {
            QualitySettings.vSyncCount = PlayerPrefs.GetInt("vsync") > 0 ? 1 : 0;
        }

        if(PlayerPrefs.HasKey("master_audio"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("master_audio");
        }
    }
    
    /// <summary>
    /// Saves the setting field values to PlayerPrefs.
    /// </summary>
    public void SaveSettings()
    {        
        // Graphics
        PlayerPrefs.SetInt("fullscreen_mode", fullscreenMode.value);
        PlayerPrefs.SetInt("resolution", resolution.value);
        PlayerPrefs.SetInt("graphics_quality", quality.value);
        PlayerPrefs.SetInt("vsync", vsync.isOn ? 1 : 0);

        // Audio
        PlayerPrefs.SetFloat("master_audio", masterAudioSlider.value);
    }
    
    /// <summary>
    /// Loads the setting field options and their current values from PlayerPrefs.
    /// </summary>
    public void LoadSettingsFields()
    {
        #region Getting Field Options
        // Fullscreen Mode Options
        fullscreenMode.ClearOptions();
        List<TMP_Dropdown.OptionData> fullscreenOptions = new List<TMP_Dropdown.OptionData>();
        foreach (FullScreenMode mode in Enum.GetValues(typeof(FullScreenMode)))
        {
            fullscreenOptions.Add(new TMP_Dropdown.OptionData(mode.ToString()));
        }
        fullscreenMode.AddOptions(fullscreenOptions);

        // Resolutions Options
        resolution.ClearOptions();
        List<TMP_Dropdown.OptionData> resolutionOptions = new List<TMP_Dropdown.OptionData>();
        foreach (Resolution res in Screen.resolutions)
        {
            resolutionOptions.Add(new TMP_Dropdown.OptionData($"{res.width}x{res.height}"));
        }
        resolution.AddOptions(resolutionOptions);

        // Quality Options
        quality.ClearOptions();
        List<TMP_Dropdown.OptionData> qualityOptions = new List<TMP_Dropdown.OptionData>();
        foreach (string qualityLevelName in QualitySettings.names)
        {
            qualityOptions.Add(new TMP_Dropdown.OptionData(qualityLevelName));
        }
        quality.AddOptions(qualityOptions);
        #endregion
        
        #region Setting Select Options
        // Set preferences
        if(PlayerPrefs.HasKey("fullscreen_mode"))
        {
            fullscreenMode.SetValueWithoutNotify(PlayerPrefs.GetInt("fullscreen_mode"));
        }
        else
        {
            fullscreenMode.SetValueWithoutNotify((int) Screen.fullScreenMode);
        }

        if(PlayerPrefs.HasKey("resolution"))
        {
            resolution.SetValueWithoutNotify(PlayerPrefs.GetInt("resolution"));
        }
        else
        {
            resolution.SetValueWithoutNotify(resolution.options.FindIndex((option) => option.text == $"{Screen.currentResolution.width}x{Screen.currentResolution.height}"));
        }

        if(PlayerPrefs.HasKey("graphics_quality"))
        {
            quality.SetValueWithoutNotify(PlayerPrefs.GetInt("graphics_quality"));
        }
        else
        {
            quality.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
        }

        if(PlayerPrefs.HasKey("graphics_quality"))
        {
            vsync.SetIsOnWithoutNotify(PlayerPrefs.GetInt("vsync") > 0 ? true : false);
        }
        else
        {
            vsync.SetIsOnWithoutNotify(QualitySettings.vSyncCount > 0 ? true : false);
        }

        if(PlayerPrefs.HasKey("master_audio"))
        {
            masterAudioSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("master_audio"));
            masterAudioSliderValue.UpdateIndicator(PlayerPrefs.GetFloat("master_audio"));

        }
        else
        {
            AudioListener.volume = 1.0f;
            masterAudioSliderValue.UpdateIndicator(1.0f);
        }
        #endregion
    }
}