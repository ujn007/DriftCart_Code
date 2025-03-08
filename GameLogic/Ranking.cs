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
                distances.Add(localPos.z); // z ���� ����� ��, ������ ��
            }
        }

        // ���� ������ ���� ���
        int rank = 1; // �⺻ ������ 1 (���� ���̶�� ����)
        foreach (float distance in distances)
        {
            if (distance > 0) // �տ� �ִ� ���� ����ŭ ������ �ڷ� �и�
            {
                rank++;
            }
        }

        Debug.Log("Current Rank: " + rank);

        // �հ� ���� ���� �� üũ
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
