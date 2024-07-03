using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/GameDifficult")]
public class GameDifficultSO : ScriptableObject
{
    public float playerSpeed;
    public float playerMaxSpeed;

    public float enemyMinSpeed;
    public float enemyMaxSpeed;
}
