using Hellmade.Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Slider musicSlider, sfxSlider;

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener((float a) => ChangeMusic());
        sfxSlider.onValueChanged.AddListener((float a) => ChangeSFX());
    }

    private void ChangeMusic()
    {
        EazySoundManager.GlobalMusicVolume = musicSlider.value;
    }

    private void ChangeSFX()
    {
        EazySoundManager.GlobalSoundsVolume = sfxSlider.value;
    }
}
