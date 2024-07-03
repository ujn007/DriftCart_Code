using System;
using UnityEngine;
using UnityEngine.Playables;

public class MainSceneChecker : MonoSingleton<MainSceneChecker>
{
    [SerializeField] private PlayableDirector menuDirector, shortMenuDirector;

    private void Start()
    {
        int loadCount = SceneLoadTracker.GetMainSceneLoadCount();

        if (loadCount >= 1)
        {
            print("MainScene이 두 번 이상 로드됬엉.");
            shortMenuDirector.Play();
        }
        else
        {
            menuDirector.Play();
        }
    }

    public void ClearMainSceneFlag()
    {
        SceneLoadTracker.ClearMainSceneLoadedFlag();
        print("MainScene 로드 플래그가 삭제!!.");
    }
}
