using UnityEngine;

public class AplicationQuit : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationQuit()
    {
        MainSceneChecker.Instance.ClearMainSceneFlag();
    }
}
