using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public FullScreenMode fullscreenMode = FullScreenMode.FullScreenWindow; 

    void Start()
    {
        if (resolutionDropdown == null)
        {
            resolutionDropdown = FindFirstObjectByType<Dropdown>();
        }

        InitializeResolutionDropdown();
    }

    void InitializeResolutionDropdown()
    {
        Resolution[] resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();

        int currentResolutionIndex = 0;
        int maxResolutionArea = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            int area = resolutions[i].width * resolutions[i].height;
            resolutionOptions.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (area > maxResolutionArea)
            {
                maxResolutionArea = area;
                currentResolutionIndex = i;
            }

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    public void OnResolutionChanged(int index)
    {
        Resolution[] resolutions = Screen.resolutions;
        if (index >= 0 && index < resolutions.Length)
        {
            Resolution selectedResolution = resolutions[index];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreenMode);
        }
    }

    public void SetToMaxResolution()
    {
        Resolution[] resolutions = Screen.resolutions;
        int maxResolutionIndex = 0;
        int maxArea = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            int area = resolutions[i].width * resolutions[i].height;
            if (area > maxArea)
            {
                maxArea = area;
                maxResolutionIndex = i;
            }
        }

        if (maxResolutionIndex >= 0 && maxResolutionIndex < resolutions.Length)
        {
            Resolution maxResolution = resolutions[maxResolutionIndex];
            Screen.SetResolution(maxResolution.width, maxResolution.height, fullscreenMode);
            resolutionDropdown.value = maxResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
    }

    //public void ToggleFullScreen(bool isFullScreen)
    //    {
    //        if (isFullScreen)
    //        {
    //            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
    //        }
    //        else
    //        {
    //            Screen.fullScreenMode = FullScreenMode.Windowed;
    //        }
    //    }
}
