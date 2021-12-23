using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mainMixer;


    //method to set volume in the settings menu - sets volume of all sound sources that have the MainAudoMixer as Output
    //(via MainAudioMixer)
    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("volumeParam", volume);
    }
}
