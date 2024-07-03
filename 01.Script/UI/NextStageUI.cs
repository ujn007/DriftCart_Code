using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextStageUI : MonoBehaviour
{
    [SerializeField] private Button nextStageBtn;
    [SerializeField] private Button menuBtn;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        nextStageBtn.onClick.AddListener(() => NextScene());
        menuBtn.onClick.AddListener(() => Menu());
    }

    private void NextScene()
    {
        if (GameManager.Instance.stage >= 4)
        {
            Menu();
            return;
        }
        GameManager.Instance.EndGame();
    }

    private void Menu()
    {
        SceneManager.LoadScene(SceneNames.MenuScene);
    }

    public void ShowUI(bool show)
    {
        if (show)
            canvasGroup.alpha = 1;
        else
            canvasGroup.alpha = 0;

        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;
    }
}
