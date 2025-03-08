using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTimer : MonoBehaviour
{
    public event Action retireEvent;

    public void CheckTime()
    {
        GameManager.Instance.StartDelayCallback(6, ()=> retireEvent?.Invoke()); 
    }
}
