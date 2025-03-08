using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpeedItem : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private float speed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Cars car))
        {
            car.GoStraight(transform.forward, car.MaxSpeed + speed, time);
        }
    }
}
