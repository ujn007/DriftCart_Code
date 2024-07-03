using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTracker : MonoBehaviour
{
    private const string MainSceneLoadedKey = "MainSceneLoaded";

    void Start()
    {
        if (SceneManager.GetActiveScene().name == SceneNames.MenuScene)
        {
            int loadCount = PlayerPrefs.GetInt(MainSceneLoadedKey, 0);
            loadCount++;
            PlayerPrefs.SetInt(MainSceneLoadedKey, loadCount);
            PlayerPrefs.Save();
        }
    }

    public static int GetMainSceneLoadCount()
    {
        return PlayerPrefs.GetInt(MainSceneLoadedKey, 0);
    }

    public static void ClearMainSceneLoadedFlag()
    {
        PlayerPrefs.DeleteKey(MainSceneLoadedKey);
        PlayerPrefs.Save();
    }
}
