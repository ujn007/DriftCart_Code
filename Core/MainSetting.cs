using System.Collections.Generic;
using UnityEngine;

public class MainSetting : MonoBehaviour
{
    [SerializeField] private List<CarRankSO> carSOList = new List<CarRankSO>();

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
        ResetSO();
    }

    private void OnApplicationQuit()
    {
        ResetSO();
    }

    private void ResetSO()
    {
        foreach (CarRankSO carRankSO in carSOList)
        {
            carRankSO.ResetInfo();
        }
    }
}
