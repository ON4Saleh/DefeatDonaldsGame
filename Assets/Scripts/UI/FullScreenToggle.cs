//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class FullScreenToggle : MonoBehaviour
//{
//    public Dropdown resolutionDropdown;

//    void Start()
//    {
//        if (resolutionDropdown == null)
//        {
//            resolutionDropdown = FindFirstObjectByType<Dropdown>(); 
//        }

//        Resolution[] resolutions = Screen.resolutions;

//        resolutionDropdown.ClearOptions();
//        List<string> resolutionOptions = new List<string>();

//        foreach (var resolution in resolutions)
//        {
//            resolutionOptions.Add(resolution.width + " x " + resolution.height);
//        }
//        resolutionDropdown.AddOptions(resolutionOptions);

//        int currentResolutionIndex = GetCurrentResolutionIndex(resolutions);
//        resolutionDropdown.value = currentResolutionIndex;
//        resolutionDropdown.RefreshShownValue();

//        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
//    }

//    private int GetCurrentResolutionIndex(Resolution[] resolutions)
//    {
//        for (int i = 0; i < resolutions.Length; i++)
//        {
//            if (resolutions[i].width == Screen.currentResolution.width &&
//                resolutions[i].height == Screen.currentResolution.height)
//            {
//                return i;
//            }
//        }
//        return 0;
//    }

//    private void OnResolutionChanged(int index)
//    {
//        Resolution[] resolutions = Screen.resolutions;
//        Resolution selectedResolution = resolutions[index];
//        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode);
//    }

//    public void ToggleFullScreen(bool isFullScreen)
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
//}
