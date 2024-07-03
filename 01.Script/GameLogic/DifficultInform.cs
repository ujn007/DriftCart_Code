using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultInform : MonoSingleton<DifficultInform>
{
    [HideInInspector] public GameDifficultSO currentDifficultSO;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public GameDifficultSO GetDifficultSO() => currentDifficultSO;
}
