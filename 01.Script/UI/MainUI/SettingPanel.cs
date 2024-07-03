using Hellmade.Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Slider musicSlider, sfxSlider;
    [SerializeField] private Toggle toggle;

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener((float a) => ChangeMusic());
        sfxSlider.onValueChanged.AddListener((float a) => ChangeSFX());
        toggle.onValueChanged.AddListener((bool a) => toggleChenge(a));
    }

    private void ChangeMusic()
    {
        EazySoundManager.GlobalMusicVolume = musicSlider.value;
    }

    private void ChangeSFX()
    {
        EazySoundManager.GlobalSoundsVolume = sfxSlider.value;
    }

    private void toggleChenge(bool a)
    {
        int rapCount = a ? 1 : 3;
        GameManager.Instance.ChangeRap(rapCount);
    }
}
