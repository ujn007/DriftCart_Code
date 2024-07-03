using Hellmade.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TST2 : MonoSingleton<TST2>
{
    [SerializeField] private AudioClip a;

    private void Awake()
    {
        EazySoundManager.PlaySound(a,true);
    }
}
