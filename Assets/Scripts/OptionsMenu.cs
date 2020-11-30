using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    public Dropdown resolutionDropdown;
    
    Resolution[] resolutions;
    
    void Start ()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int a = 0; a < resolutions.Length; a++)
        {
            string option = resolutions[a].width + " x " + resolutions[a].height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);
    }
    
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
