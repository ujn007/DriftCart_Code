using Cinemachine;
using Hellmade.Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public event Action OnGameStart;
    public event Action OnGameEnd;

    public CinemachineVirtualCamera cam;
    public int stage { get; private set; } = 1;

    [Header("GameSet")]
    public List<Transform> spawnPoints;
    public List<Cars> cars;
    public Transform carParent;
    [HideInInspector] public List<int> rankingList;
    public int rapMaxCount;

    public CheckGoal checkGoal;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI startGameCountText;
    [SerializeField] private TextMeshPro timeText;
    [SerializeField] private GameObject allUI;
    public GameObject gameUI;
    public NextStageUI nextPanel;
    public PauseUI pausePanel;
    public RankingUI rankUI;
    [HideInInspector] public Image boostImg;
    [HideInInspector] public bool pauseToggle;

    public CinemachineImpulseSource impulseSource { get; private set; }
    [HideInInspector] public CarController playerCar;

    [SerializeField] private GameObject rankScene;

    private void Awake()
    {
        boostImg = GameObject.Find("BoostBar").GetComponent<Image>();
        impulseSource = cam.GetComponent<CinemachineImpulseSource>();

        //SettingCars(true);
    }

    public void SettingCars(bool first = false)
    {
        if (!first)
            DestroyAllCars();
        else
            ResetCarSORank();

        cars = cars.OrderBy(car => car.rankSO.rank).ToList();

        for (int i = 0; i < cars.Count; i++)
        {
            Cars car = Instantiate(cars[i], carParent);

            car.transform.position = spawnPoints[i].position;

            if (car.TryGetComponent(out CarController playerCar))
            {
                this.playerCar = playerCar;
                playerCar.SetSpeed();
            }
        }
        cam.Follow = playerCar.transform;
    }

    private void ResetCarSORank()
    {
        foreach (Cars car in cars)
        {
            car.rankSO.rank = 0;
        }
    }

    private void DestroyAllCars()
    {
        foreach (Transform child in carParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void StartGameCount(float time)
    {
        StartCoroutine(StartCountCoroutine(time));
    }

    private IEnumerator StartCountCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        int count = 3;
        AudioManager.Instance.PlaySFX("Beap", 1);

        while (count > 0)
        {
            startGameCountText.text = count.ToString();
            yield return new WaitForSeconds(1);
            count--;
        }

        startGameCountText.text = "Start!!";
        InvokeGameStart();

        yield return new WaitForSeconds(0.5f);
        MapLoader.Instance.StartMusic();

        yield return new WaitForSeconds(0.5f);
        startGameCountText.text = "";
    }

    public void InvokeGameStart()
    {
        OnGameStart?.Invoke();
    }

    public void EndGame()
    {
        FadeUI.Instance.Fadeout(5, NextStage);
    }

    public void NextStage()
    {
        ++stage;
        nextPanel.ShowUI(false);

        print(stage);
        if (stage >= 4)
        {
            EazySoundManager.StopAll();
            DestroyAllCars();

            MoveRankScene();
            return;
        }

        gameUI.SetActive(true);
        ResetSetting();
    }

    private void ResetSetting()
    {
        foreach (Cars car in cars)
        {
            car.CheckMiddleCollider(false);
        }
        MapLoader.Instance.SpawnMap(stage);
        EazySoundManager.StopAll();

        checkGoal.ResetGame();
        rankUI.ResetRankingUI();
        PlayerReset();
        SettingCars();
    }

    private void PlayerReset()
    {
        cam.Priority = 11;
        playerCar.carMode = CarMode.Normal;
    }

    public void MoveRankScene()
    {
        playerCar.EndCarSet();
        playerCar.ChangeCarLayer(false);

        rankScene.SetActive(true);
        allUI.SetActive(false);

        checkGoal.rapText.enabled = false;

        cam.Priority = 5;
        OnGameEnd?.Invoke();
    }

    public void ChangeRap(int n) => rapMaxCount = n;

    public Coroutine StartDelayCallback(float time, Action Callback)
    {
        return StartCoroutine(DelayCoroutine(time, Callback));
    }

    private IEnumerator DelayCoroutine(float time, Action Callback)
    {
        yield return new WaitForSeconds(time);
        Callback?.Invoke();
    }

    private void OnApplicationQuit()
    {
        foreach (Cars car in cars)
        {
            car.rankSO.point = 0;
        }
    }
}
