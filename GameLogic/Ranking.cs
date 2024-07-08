using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    [SerializeField] private Cars[] cars;
    private List<float> distances = new List<float>();
    private CarController playerCar;

    private void Awake()
    {
        playerCar = GameManager.Instance.playerCar;
        cars = FindObjectsOfType<Cars>();
    }

    void Update()
    {
        CheckPositionAndRank();
    }

    void CheckPositionAndRank()
    {
        foreach (Cars car in cars)
        {
            if (car != playerCar)
            {
                Vector3 localPos = playerCar.transform.InverseTransformPoint(car.transform.position);
                distances.Add(localPos.z); // z 값이 양수면 앞, 음수면 뒤
            }
        }

        // 현재 차량의 순위 계산
        int rank = 1; // 기본 순위는 1 (가장 앞이라고 가정)
        foreach (float distance in distances)
        {
            if (distance > 0) // 앞에 있는 차량 수만큼 순위가 뒤로 밀림
            {
                rank++;
            }
        }

        Debug.Log("Current Rank: " + rank);

        // 앞과 뒤의 차량 수 체크
        int frontCars = 0;
        int backCars = 0;

        foreach (float distance in distances)
        {
            if (distance > 0)
            {
                frontCars++;
            }
            else if (distance < 0)
            {
                backCars++;
            }
        }

        Debug.Log("Cars in Front: " + frontCars);
        Debug.Log("Cars Behind: " + backCars);
    }
}
