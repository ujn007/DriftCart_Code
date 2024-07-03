using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMiddleCol : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Cars car))
        {
            car.CheckMiddleCollider(true);
        }
    }
}
