using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartBtn : MonoBehaviour
{
    [SerializeField] private List<Button> mods;
    [SerializeField] private List<GameDifficultSO> difficultSO;

    private void Awake()
    {
        mods[0].onClick.AddListener(() => GoToGameScene(0));
        mods[1].onClick.AddListener(() => GoToGameScene(1));
        mods[2].onClick.AddListener(() => GoToGameScene(2));
    }

    private void GoToGameScene(int n)
    {
        DifficultInform.Instance.currentDifficultSO = difficultSO[n];
        AsyncLoader.Instance.LoadLevelButton(SceneNames.GameScene);
    }
}
