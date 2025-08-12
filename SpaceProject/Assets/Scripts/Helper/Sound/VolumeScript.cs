using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeScript : MonoBehaviour
{
    public AudioMixer mixer;
    Resolution[] resolutions;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public void Start() {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        int currentResoIndex = 0;
        List<string> options = new List<string>();
        for (int i=0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "hz";
            options.Add(option);
            if (resolutions[i].width == Screen.width && 
                resolutions[i].height == Screen.height) 
            { currentResoIndex = 1; }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResoIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void volumeChange(float volume) {
        mixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }

    public void setQuality(int qualIndex)
    {
        QualitySettings.SetQualityLevel(qualIndex);
    }
    public void setReso(int resoIndex)
    {
        Resolution resolution = resolutions[resoIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void toggleFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
