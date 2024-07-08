using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RankScene : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CarController car;

    private TextMeshPro rankTxt;

    private void Awake()
    {
         rankTxt = GetComponentInChildren<TextMeshPro>();
    }

    private void OnEnable() 
    {
        GameManager.Instance.OnGameEnd += HandleGameEnd;
    }

    private void OnDestroy()
    {
        
        GameManager.Instance.OnGameEnd -= HandleGameEnd;
    }

    private void HandleGameEnd()
    {
        print("d¤·³Ä¤Ä·Ð¾ßÇô¤ÇÇô¤Á¤¤°íµ®¤ÁÃC¤¡¤¦");
        MapLoader.Instance.ClearSpawnObject();
        CarController playerCar = Instantiate(car);

        playerCar.transform.SetPositionAndRotation(spawnPoint.position, Quaternion.LookRotation(spawnPoint.forward));
        rankTxt.text = playerCar.rankSO.rank.ToString();
        AudioManager.Instance.StopAllSound();

        AudioManager.Instance.PlaySFX("Win", 1);
        GameManager.Instance.StartDelayCallback(10, () => EndGame());
    }

    private void EndGame()
    {
        GameManager.Instance.nextPanel.ShowUI(true);
    }
}
