using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsUpdater : MonoBehaviour
{
    Toggle musictgl, sfxtgl;
    public AudioMixer mixer;

    private void Start()
    {
        musictgl = GameObject.Find("Music Toggle").GetComponent<Toggle>();
        sfxtgl   = GameObject.Find("SFX Toggle").GetComponent<Toggle>();
        //if you back to mainmenu
        if (Settings.Music)
            musictgl.isOn = true;
        else
            musictgl.isOn = false;
        if (Settings.SFX)
            sfxtgl.isOn = true;
        else
            sfxtgl.isOn = false;
    }

    //toggle music & sfx on\off
    public void ChangeSFX()
    {
        if (sfxtgl)
        {
            if (sfxtgl.isOn)
            {
                Settings.SFX = true;
                mixer.SetFloat("SFXVolume", 0f);
            }
            else
            {
                Settings.SFX = false;
                mixer.SetFloat("SFXVolume", -80f);
            }
        }
    }

    public void ChangeMusic()
    {
        if (musictgl)
        {
            if (musictgl.isOn)
            {
                Settings.Music = true;
                mixer.SetFloat("MusicVolume", 0f);
            }
            else
            {
                Settings.Music = false;
                mixer.SetFloat("MusicVolume", -80f);
            }
        }
    }
}
