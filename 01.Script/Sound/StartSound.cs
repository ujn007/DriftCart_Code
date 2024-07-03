using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSound : MonoBehaviour
{
    [SerializeField] private string startMusicName;
    [SerializeField] private float valume;

    private void OnEnable()
    {
        StartMusic(valume);
    }

    public void StartMusic(float v)
    {
        AudioManager.Instance.PlayMusic(startMusicName, v);
    }
}
