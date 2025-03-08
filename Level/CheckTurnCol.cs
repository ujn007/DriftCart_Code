using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckTurnCol : MonoBehaviour
{
    [HideInInspector] public List<BoxCollider> checkCols;

    private void Awake()
    {
        checkCols = GetComponentsInChildren<BoxCollider>().ToList();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Cars car))
        {
            car.MoveToNextTrm(this);
        }
    }
}
