using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class StartGameTimeLine : MonoBehaviour
{
    [SerializeField] private List<TimelineAsset> trackTimeLines = new List<TimelineAsset>();
    private PlayableDirector timeLine;

    private void Awake()
    {
        timeLine = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        MapLoader.Instance.loadMapComplete += HandleStartTimeLine;
        timeLine.stopped += HandleEndTimeLine;
    }

    private void OnDestroy()
    {
        MapLoader.Instance.loadMapComplete -= HandleStartTimeLine;
        timeLine.stopped -= HandleEndTimeLine;
    }

    private void HandleStartTimeLine(int stage)
    {
        GameManager.Instance.SettingCars(true);

        timeLine.playableAsset = trackTimeLines[stage - 1];
        timeLine.Play();
    }

    private void HandleEndTimeLine(PlayableDirector director)
    {
        GameManager.Instance.StartGameCount(1);
        print("³¡³µÀ½");
    }
}
