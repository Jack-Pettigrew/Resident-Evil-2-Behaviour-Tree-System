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
    
    public void ResetSettings()
    {
        PlayerPrefs.DeleteAll();

        LoadSettings();
        ApplySettings();
    }
    
    public void ApplySettings()
    {
        Screen.SetResolution(Screen.resolutions[resolution.value].width, Screen.resolutions[resolution.value].height, (FullScreenMode)fullscreenMode.value);
        QualitySettings.SetQualityLevel(quality.value, true);
        QualitySettings.vSyncCount = vsync.isOn ? 1 : 0;
        
        SaveSettings();
    }
    
    public void SaveSettings()
    {        
        PlayerPrefs.SetInt("fullscreen_mode", fullscreenMode.value);
        PlayerPrefs.SetInt("resolution", resolution.value);
        PlayerPrefs.SetInt("graphics_quality", quality.value);
        PlayerPrefs.SetInt("vsync", vsync.isOn ? 1 : 0);
    }
    
    public void LoadSettings()
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
        #endregion
    }
}