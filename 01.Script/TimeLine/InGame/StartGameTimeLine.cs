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
        timeLine.stopped += HandleEndTimeLine;
        MapLoader.Instance.loadMapComplete += HandleStartTimeLine;
    }

    private void OnDisable()
    {
        timeLine.stopped -= HandleEndTimeLine;
        MapLoader.Instance.loadMapComplete -= HandleStartTimeLine;
    }

    private async void HandleStartTimeLine(int stage)
    {
        if (stage != 1)
            await Task.Delay(1500);

        timeLine.playableAsset = trackTimeLines[stage -1];
        timeLine.Play();
    }

    private void HandleEndTimeLine(PlayableDirector director)
    {
        GameManager.Instance.StartGameCount(1);
        print("³¡³µÀ½");
    }
}
