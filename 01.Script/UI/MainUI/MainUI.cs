using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button outButton;
    [SerializeField] private Button timeLine;
    [SerializeField] private Button TutorialBtn;

    [SerializeField] private List<RectTransform> panels;

    private void Awake()
    {
        startButton.onClick.AddListener(() => OnPanel(1));
        settingButton.onClick.AddListener(() => OnPanel(2));
        outButton.onClick.AddListener(() => ExitGame());
        timeLine.onClick.AddListener(() => ShowTimeLine());
        TutorialBtn.onClick.AddListener(()=> ShowTuto(2));
    }

    private void OnPanel(int n)
    {
        panels[n-1].gameObject.SetActive(true);
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ShowTuto(int n)
    {
        panels[n].gameObject.SetActive(true);   
    }

    private void ShowTimeLine()
    {
        MainSceneChecker.Instance.ClearMainSceneFlag();
        SceneManager.LoadScene(SceneNames.MenuScene);
    }
}
