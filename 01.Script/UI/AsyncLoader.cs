using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AsyncLoader : MonoSingleton<AsyncLoader>
{
    [SerializeField] private List<string> messageList = new();

    [SerializeField] private GameObject loadingScene;

    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI messageTxt;

    public void LoadLevelButton(string levelToLoad)
    {
        loadingScene.SetActive(true);

        RandomMessage();
        StartCoroutine(LoadLevelAsync(levelToLoad));
    }

    private IEnumerator LoadLevelAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);    

        while (!loadOperation.isDone)
        {
            float prograssValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = prograssValue;
            yield return null;
        }
    }

    private void RandomMessage()
    {
        int randIndex = Random.Range(0, messageList.Count);
        messageTxt.text = $"그거 아시나요?\n {messageList[randIndex]}";
    }
}
