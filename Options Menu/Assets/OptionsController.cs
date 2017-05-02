using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class OptionsController : MonoBehaviour {

    public Toggle fullScreenToggle;
    public Dropdown resolutionDropDown;
    public Dropdown textureQualityDropDown;
    public Dropdown aaDropDown;
    public Dropdown vSyncDropDown;
    public Slider masterVolumeSlider;
    public Button applyButton;

    public Resolution[] resolutions;
    public OptionsSettings optionsSettings;

    void OnEnable()
    {
        optionsSettings = new OptionsSettings();

        fullScreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropDown.onValueChanged.AddListener(delegate { OnResolutionChanged(); });
        textureQualityDropDown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        aaDropDown.onValueChanged.AddListener(delegate { OnAAChange(); });
        vSyncDropDown.onValueChanged.AddListener(delegate { OnVSynceChange(); });
        masterVolumeSlider.onValueChanged.AddListener(delegate { OnMasterVolumeChange(); });
        applyButton.onClick.AddListener(delegate { OnApply(); });

        resolutions = Screen.resolutions;
        foreach(Resolution res in resolutions)
        {
            resolutionDropDown.options.Add(new Dropdown.OptionData(res.ToString()));
        }

        LoadSettings();
    }

    public void OnFullscreenToggle()
    {
        optionsSettings.fullScreen =  Screen.fullScreen = fullScreenToggle.isOn;
    }

    public void OnResolutionChanged()
    {
        Screen.SetResolution(resolutions[resolutionDropDown.value].width, resolutions[resolutionDropDown.value].height, Screen.fullScreen);
        optionsSettings.resolutionIndex = resolutionDropDown.value;
    }

    public void OnTextureQualityChange()
    {
        QualitySettings.masterTextureLimit =  optionsSettings.textureQuality = textureQualityDropDown.value;
    }

    public void OnAAChange()
    {
        QualitySettings.antiAliasing = (int)Mathf.Pow(2f, aaDropDown.value);
        optionsSettings.aa = aaDropDown.value;
    }

    public void OnVSynceChange()
    {
        QualitySettings.vSyncCount = optionsSettings.vSync = vSyncDropDown.value;
    }

    public void OnMasterVolumeChange()
    {
        //FMOD master Volume control...

        //Might need a hacky way to turn down the volume...

    }

    public void OnApply()
    {
        SaveSettings();
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(optionsSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/GameSettings.json", jsonData);
    }

    public void LoadSettings()
    {
        optionsSettings = JsonUtility.FromJson<OptionsSettings>(File.ReadAllText(Application.persistentDataPath + "/GameSettings.json"));

        if (optionsSettings != null)
        {
           
            fullScreenToggle.isOn = optionsSettings.fullScreen;
            resolutionDropDown.value = optionsSettings.resolutionIndex;
            textureQualityDropDown.value = optionsSettings.textureQuality;
            aaDropDown.value = optionsSettings.aa;
            vSyncDropDown.value = optionsSettings.vSync;
            masterVolumeSlider.value = optionsSettings.masterVolume;

            resolutionDropDown.RefreshShownValue();
        }
    }

}
