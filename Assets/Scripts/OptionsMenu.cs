using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that manages all settings in the OptionsMenu
/// </summary>
public class OptionsMenu : MonoBehaviour
{
    /// <summary>
    /// Reference of the GameSettings tab of the OptionsMenu
    /// </summary>
    [SerializeField] private GameObject gameSettingsTab;
    /// <summary>
    /// Reference of the Graphics Settings tab of the OptionsMenu
    /// </summary>
    [SerializeField] private GameObject graphicsTab;
    /// <summary>
    /// Reference of the Audio Settings tab of the OptionsMenu
    /// </summary>
    [SerializeField] private GameObject audioTab;

    /// <summary>
    /// List of all the toggles in the Options Menu
    /// </summary>
    private List<Toggle> toggleList = new List<Toggle>();

    private string resolution;
    private int antiAliasing;
    private int isAnisotropicFilteringOn;
    private int isVSyncOn;
    private int targetFrameLimit;
    private int depthBuffer;

    private void Awake()
    {
        // get all the toggles
        toggleList = GetComponentsInChildren<Toggle>(true).ToList();

        // try to get saved settings values from the system, if not choose the default value
        resolution = PlayerPrefs.GetString("Resolution", "1920x1080");
        antiAliasing = PlayerPrefs.GetInt("AntiAliasing", 0);
        isAnisotropicFilteringOn = PlayerPrefs.GetInt("AnisotropicFiltering", 0);
        isVSyncOn = PlayerPrefs.GetInt("VSync", 0);
        targetFrameLimit = PlayerPrefs.GetInt("TargetFrameLimit", -1);
        depthBuffer = PlayerPrefs.GetInt("DepthBuffer", 32);
    }

    private void OnEnable()
    {
        // Toggle on the right one in the group for each setting
        // Set Resolution Toggle on
        switch (resolution)
        {
            case "3840x2160": toggleList.FirstOrDefault(toggle => toggle.name == "3840x2160Toggle").isOn = true; break;
            case "1920x1080": toggleList.FirstOrDefault(toggle => toggle.name == "1920x1080Toggle").isOn = true; break;
            case "1280x720": toggleList.FirstOrDefault(toggle => toggle.name == "1280x720Toggle").isOn = true; break;
            case "1024x768": toggleList.FirstOrDefault(toggle => toggle.name == "1024x768Toggle").isOn = true; break;
        }

        // Set AntiAliasing toggle on
        switch (antiAliasing)
        {
            case 0: toggleList.FirstOrDefault(toggle => toggle.name == "AntiAliasingOffToggle").isOn = true; break;
            case 2: toggleList.FirstOrDefault(toggle => toggle.name == "AntiAliasing2xToggle").isOn = true; break;
            case 4: toggleList.FirstOrDefault(toggle => toggle.name == "AntiAliasing4xToggle").isOn = true; break;
            case 8: toggleList.FirstOrDefault(toggle => toggle.name == "AntiAliasing8xToggle").isOn = true; break;
            case 16: toggleList.FirstOrDefault(toggle => toggle.name == "AntiAliasing16xToggle").isOn = true; break;
        }


        // Set Anisotropic Filtering toggle on
        switch (isAnisotropicFilteringOn)
        {
            case 0: toggleList.FirstOrDefault(toggle => toggle.name == "AnisotropicDisableToggle").isOn = true; break;
            case 1: toggleList.FirstOrDefault(toggle => toggle.name == "AnisotropicEnableToggle").isOn = true; break;
        }

        // Set VSync toggle on
        switch (isVSyncOn)
        {
            case 0: toggleList.FirstOrDefault(toggle => toggle.name == "VSyncDisableToggle").isOn = true; break;
            case 1: toggleList.FirstOrDefault(toggle => toggle.name == "VSyncEnableToggle").isOn = true; break;
        }

        // Set FrameRate limit toggle on
        switch (targetFrameLimit)
        {
            case -1: toggleList.FirstOrDefault(toggle => toggle.name == "FrameRateLimitOffToggle").isOn = true; break;
            case 30: toggleList.FirstOrDefault(toggle => toggle.name == "FrameRateLimit30Toggle").isOn = true; break;
            case 60: toggleList.FirstOrDefault(toggle => toggle.name == "FrameRateLimit60Toggle").isOn = true; break;
            case 90: toggleList.FirstOrDefault(toggle => toggle.name == "FrameRateLimit90Toggle").isOn = true; break;
            case 120: toggleList.FirstOrDefault(toggle => toggle.name == "FrameRateLimit120Toggle").isOn = true; break;
        }

        // Set the depth buffer toggle on
        switch (depthBuffer)
        {
            case 16: toggleList.FirstOrDefault(toggle => toggle.name == "Depth16Toggle").isOn = true; break;
            case 32: toggleList.FirstOrDefault(toggle => toggle.name == "Depth32Toggle").isOn = true; break;
        }

        SelectGraphicsTab();            
    }

    /// <summary>
    /// Opens the Graphics Settings tab
    /// </summary>
    public void SelectGraphicsTab()
    {
        gameSettingsTab.SetActive(false);
        graphicsTab.SetActive(true);
        audioTab.SetActive(false);
    }

    /// <summary>
    /// Opens the GameSettings tab
    /// </summary>
    public void SelectGameSettingsTab()
    {
        gameSettingsTab.SetActive(true);
        graphicsTab.SetActive(false);
        audioTab.SetActive(false);
    }

    /// <summary>
    /// Opens the Audio Settings tab
    /// </summary>
    public void SelectAudioTab()
    {
        gameSettingsTab.SetActive(false);
        graphicsTab.SetActive(false);
        audioTab.SetActive(true);
    }

    /// <summary>
    /// Applies the Graphics Settings selected to the game 
    /// </summary>
    public void ApplyGraphics()
    {
        // UPDATE RESOLUTION
        int separator = resolution.IndexOf("x");
        string horizontalRes = resolution.Substring(0, separator);
        string verticalRes = resolution.Substring(separator + 1);

        Screen.SetResolution(int.Parse(horizontalRes), int.Parse(verticalRes), true);

        // UPDATE ANTIALIASING
        QualitySettings.antiAliasing = antiAliasing;

        // UPDATE ANISOTROPIC FILTERING
        QualitySettings.anisotropicFiltering = isAnisotropicFilteringOn == 0 ? AnisotropicFiltering.Disable : AnisotropicFiltering.ForceEnable;

        // UPDATE VSYNC
        QualitySettings.vSyncCount = isVSyncOn == 0 ? 0 : 1;

        // UPDATE TARGET FRAMERATE
        Application.targetFrameRate = targetFrameLimit;
    }


    #region Graphics Settings

    #region ResolutionToggles
    /// <summary>
    /// Set the resolution 3840x2160 toggle on when selected
    /// </summary>
    public void SetResolution3840x2160(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("resolution", "3840x2160");
            resolution = "3840x2160";
        }
    }

    /// <summary>
    /// Set the resolution 1920x1080 toggle on when selected
    /// </summary>
    public void SetResolution1920x1080(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("resolution", "1920x1080");
            resolution = "1920x1080";
        }
    }

    /// <summary>
    /// Set the resolution 1280x720 toggle on when selected
    /// </summary>
    public void SetResolution1280x720(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("resolution", "1280x720");
            resolution = "1280x720";
        }
    }

    /// <summary>
    /// Set the resolution 1024x768 toggle on when selected
    /// </summary>
    public void SetResolution1024x768(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("resolution", "1024x768");
            resolution = "1024x768";
        }
    }
    #endregion

    #region AntiAliasing

    /// <summary>
    /// Set the AntiAliasing Off toggle on when selected
    /// </summary>
    public void SetAntiAliasingOff(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("AntiAliasing", 0);
            antiAliasing = 0;
        }
    }

    /// <summary>
    /// Set the AntiAliasing 2x toggle on when selected
    /// </summary>
    public void SetAntiAliasing2x(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("AntiAliasing", 2);
            antiAliasing = 2;
        }
    }

    /// <summary>
    /// Set the AntiAliasing 4x toggle on when selected
    /// </summary>
    public void SetAntiAliasing4x(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("AntiAliasing", 4);
            antiAliasing = 4;
        }
    }

    /// <summary>
    /// Set the AntiAliasing 8x toggle on when selected
    /// </summary>
    public void SetAntiAliasing8x(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("AntiAliasing", 8);
            antiAliasing = 8;
        }
    }

    /// <summary>
    /// Set the AntiAliasing 16x toggle on when selected
    /// </summary>
    public void SetAntiAliasing16x(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("AntiAliasing", 16);
            antiAliasing = 16;
        }
    }

    #endregion

    #region AnisotropicFiltering

    /// <summary>
    /// Set the AnisotropicFiltering Enable toggle on when selected
    /// </summary>
    public void SetAnisotropicFilteringOn(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("AnisotropicFiltering", 1);
            isAnisotropicFilteringOn = 1;
        }
    }

    /// <summary>
    /// Set the AnisotropicFiltering Disable toggle on when selected
    /// </summary>
    public void SetAnisotropicFilteringOff(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("AnisotropicFiltering", 0);
            isAnisotropicFilteringOn = 0;
        }
    }

    #endregion

    #region VSyncToggles

    /// <summary>
    /// Set the VSync Enable toggle on when selected
    /// </summary>
    public void SetVSyncOn(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("VSync", 1);
            isVSyncOn = 1;
        }
    }

    /// <summary>
    /// Set the VSync Disable toggle on when selected
    /// </summary>
    public void SetVSyncOff(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("VSync", 0);
            isVSyncOn = 0;
        }
    }

    #endregion

    #region Target FrameRate

    /// <summary>
    /// Set the FrameRateLimit Off toggle on when selected
    /// </summary>
    public void SetTargetFrameRateOff(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("TargetFrameLimit", -1);
            targetFrameLimit = -1;
        }
    }

    /// <summary>
    /// Set the FrameRateLimit 30 toggle on when selected
    /// </summary>
    public void SetTargetFrameRate30(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("TargetFrameLimit", 30);
            targetFrameLimit = 30;
        }
    }

    /// <summary>
    /// Set the FrameRateLimit 60 toggle on when selected
    /// </summary>
    public void SetTargetFrameRate60(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("TargetFrameLimit", 60);
            targetFrameLimit = 60;
        }
    }

    /// <summary>
    /// Set the FrameRateLimit 90 toggle on when selected
    /// </summary>
    public void SetTargetFrameRate90(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("TargetFrameLimit", 90);
            targetFrameLimit = 90;
        }
    }

    /// <summary>
    /// Set the FrameRateLimit 120 toggle on when selected
    /// </summary>
    public void SetTargetFrameRate120(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("TargetFrameLimit", 120);
            targetFrameLimit = 120;
        }
    }


    #endregion

    #region DepthBuffer

    /// <summary>
    /// Set the DepthBuffer 16bit toggle on when selected
    /// </summary>
    public void SetDepthBuffer16bit(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("DepthBuffer", 16);
            depthBuffer = 16;
        }
    }

    /// <summary>
    /// Set the DepthBuffer 32bit toggle on when selected
    /// </summary>
    public void SetDepthBuffer32bit(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("DepthBuffer", 32);
            depthBuffer = 32;
        }
    }

    #endregion

    #endregion

    #region Game Settings

    // no game settings yet

    #endregion

    #region Audio Settings

    // no audio settings yet

    #endregion
}
