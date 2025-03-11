using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class SettingsManager : MonoBehaviour

public AudioMixer audioMixer;

Resolution[] resolutions;

public Dropdown resolutionDropdown;

//Audio settings

public void SetVolume (float volume) // for volume slider
    {
        audioMixer.SetFloat("volume", volume) 

        Debug.log(volume);
    }
    // graphics quality settings

public void SetQuality (int qualityIndex)
    {
        QualitySettings.setQualityLevel(qualityIndex);//accesses the index for the graphics quality
    }

public void SetFullscreen (bool isFullscreen)
    { 
        Screen.fullScreen = isFullscreen;

    }


    void start ()
    {
      resolutions = Screen.resolutions;

      resolutionDropdown.ClearOptions();

      List<string> options = new list<string>();

    int currentResolutionIndex = 0;


        for (int i = 0; i < resolutions.length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height ;
            options.Add(option);
         

            //what this does is that it checks the values of the current resolutions then >>
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;//stores it here <<
            }
        }

      resolutionDropdown.AddOptions(options);
      resolutionDropdown
    }