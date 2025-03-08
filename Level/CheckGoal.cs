using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CheckGoal : MonoBehaviour
{
    public List<Cars> goalCars { get; private set; } = new();

    [SerializeField] private TextMeshProUGUI finishText;
    [SerializeField] private RapTween rapUI;
    public TextMeshProUGUI rapText;
    private List<Cars> retireCars = new();

    private GameManager gm => GameManager.Instance;
    private GoalTimer goalTimer;

    private int ranking = 0;
    private int increasePoint = 6;
    private bool goal;

    private void Awake()
    {
        goalTimer = GetComponent<GoalTimer>();
    }

    private void OnEnable()
    {
        goalTimer.retireEvent += HandleRetireEvent;
    }

    private void OnDestroy()
    {
        goalTimer.retireEvent -= HandleRetireEvent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Cars car) && !goal && car.canGoal)
        {
            Rank(car);
            if (other.TryGetComponent(out CarController playerCar))
            {
                HandleRapCount(playerCar);
            }
        }
    }

    private void HandleRapCount(CarController car)
    {
        rapText.text = car.rapCount.ToString();

        if (car.rapCount >= gm.rapMaxCount)
        {
            print("끝났어");
            SetGoal(car);
        }
        else
        {
            car.CheckMiddleCollider(false);
            rapUI.MoveTweenUI();
        }
    }

    private async void SetGoal(CarController car)
    {
        finishText.text = "<shake>FINISH!";
        StartAutoMove(car);
        goalTimer.CheckTime();
        gm.gameUI.SetActive(false);

        await Task.Delay(1000);
        gm.cam.Priority = 1;
        finishText.text = "";
    }

    private void StartAutoMove(CarController car)
    {
        car.carMode = CarMode.Auto;
        car.AutoSetting();
    }

    private async void Rank(Cars car)
    {
        car.rapCount++;
        if (car.rapCount < gm.rapMaxCount) return;

        print(goalCars);
        car.ChangeCarLayer(true);
        car.CheckMiddleCollider(false);

        UpdateRanking(car);
        increasePoint--;

        if (goalCars.Count >= 6)
        {
            print("다 들어왔다");
            await EndRace();
        }
    }

    private void UpdateRanking(Cars car)
    {
        ranking++;
        car.rankSO.rank = ranking;
        goalCars.Add(car);

        gm.rankUI.SetRanking(ranking, increasePoint, car.rankSO.point, car);
    }

    private void HandleRetireEvent()
    {
        print("aaaaaaaaaaa");
        foreach (Transform carParent in GameManager.Instance.carParent)
        {
            if (carParent.TryGetComponent(out Cars car))
            {
                if (car.gameObject.layer != LayerMask.NameToLayer("GhostCar"))
                {
                    print(LayerMask.LayerToName(car.gameObject.layer));
                    car.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(transform.forward));
                }
            }
        }
    }

    private async Task EndRace()
    {
        goal = true;
        await Task.Delay(2000);
        gm.rankUI.ShowRanking();
        increasePoint = 6;
    }

    public void ResetGame()
    {
        ranking = 0;
        goal = false;
        foreach (Cars car in goalCars)
        {
            car.rapCount = 0;
            car.CheckMiddleCollider(false);
            car.ChangeCarLayer(false);
        }
        goalCars.Clear();
    }
}
