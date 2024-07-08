using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    [SerializeField] private CarController carController;
    [SerializeField] private Transform spawnPos;

    public void SpanPlayerCar()
    {
        CarController car = Instantiate(carController, spawnPos.transform.position, Quaternion.LookRotation(spawnPos.forward));
        GameManager.Instance.StartGameCount(1);

        GameManager.Instance.cam.Follow = car.transform;
        car.SetSpeed();
    }

    
}
